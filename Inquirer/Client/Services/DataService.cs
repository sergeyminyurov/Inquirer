using Inquirer.Data;
using Inquirer.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Inquirer.Services
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.json.httpclientjsonextensions?view=net-5.0
    public sealed class DataService : IDataService
    {
        const int ExpirationInterval = 300;
        private HttpClient Http { get; }
        private IMemoryCache Cache { get; }
        private ILogger<DataService> Logger { get; }
        public DataService(HttpClient http, IMemoryCache cache, ILogger<DataService> logger)
        {
            Http = http;
            Cache = cache;
            Logger = logger;
        }

        private string GetRequestUri<T>(object arg = null) =>
             $"api/{typeof(T).Name.ToLower()}/{arg}";

        public async Task<List<T>> GetList<T>() where T : Entity
        {
            try
            {
                return await Cache.GetOrCreateAsync(typeof(T), async e =>
                {
                    e.SetOptions(new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ExpirationInterval)
                    });

                    string requestUri = GetRequestUri<T>("list");
                    Logger.LogDebug($"GetList<{typeof(T).Name}>: {requestUri}");
                    return await Http.GetFromJsonAsync<List<T>>(requestUri);
                });
            }
            catch (Exception ex)
            {
                Logger.LogError($"GetList<{typeof(T).Name}>: {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task<T> Get<T>(int id) where T : Entity
        {
            try
            {
                string requestUri = GetRequestUri<T>(id);
                Logger.LogDebug($"Get<{typeof(T).Name}>: {requestUri}");
                return await Http.GetFromJsonAsync<T>(requestUri);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Get<{typeof(T).Name}>: id={id}; {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task<T> AddNew<T>(T entity) where T : Entity
        {
            try
            {
                string requestUri = GetRequestUri<T>();
                Logger.LogDebug($"AddNew<{typeof(T).Name}>: {requestUri}");
                T entity2 = (T)entity.ForDatabase();
                var result = await Http.PostAsJsonAsync<T>(requestUri, entity2);
                entity = await result.Content.ReadFromJsonAsync<T>();
                Reset<T>();
                return entity;
            }
            catch (Exception ex)
            {
                Logger.LogError($"AddNew<{typeof(T).Name}>: {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task Update<T>(T entity) where T : Entity
        {
            try
            {
                string requestUri = GetRequestUri<T>();
                Logger.LogDebug($"Update<{typeof(T).Name}>: {requestUri}");
                T entity2 = (T)entity.ForDatabase();
                await Http.PutAsJsonAsync<T>(requestUri, entity2);
                Reset<T>();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Update<{typeof(T).Name}>: {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task Delete<T>(int id) where T : Entity
        {
            try
            {
                string requestUri = GetRequestUri<T>(id);
                Logger.LogDebug($"Delete<{typeof(T).Name}>: {requestUri}");
                await Http.DeleteAsync(requestUri);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Delete<{typeof(T).Name}>: {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task Swap<T>(T entity1, T entity2) where T : Entity, IOrderedEntity
        {
            try
            {
                string requestUri = GetRequestUri<T>();
                Logger.LogDebug($"Swap<{typeof(T).Name}>: {requestUri}");
                SwapRequestData<T> value = new() 
                { 
                    Entity1 = entity1,
                    Entity2 = entity2,
                };
                await Http.PutAsJsonAsync(requestUri, value);
                int number = entity1.Number;
                entity1.Number = entity2.Number;
                entity2.Number = number;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Update<{typeof(T).Name}>: {ex.GetBaseException().Message}");
                throw;
            }
        }

        private void Reset<T>() where T : Entity => Cache.Remove(typeof(T));
    }
}