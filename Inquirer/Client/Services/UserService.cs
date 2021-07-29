using Inquirer.Data;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Inquirer.Services
{
    public sealed class UserService : IUserService
    {
        HttpClient Http { get; }
        IMemoryCache Cache { get; }
        public UserService(HttpClient http, IMemoryCache cache)
        {
            Http = http;
            Cache = cache;
        }

        private string GetRequestUri(object arg = null) =>$"api/{nameof(ApplicationUser).ToLower()}/{arg}";

        public async Task<List<ApplicationUser>> GetList()
        {
            try
            {
                return await Cache.GetOrCreateAsync(typeof(ApplicationUser), async e =>
                {
                    e.SetOptions(new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
                    });

                    string requestUri = GetRequestUri("list");
#if DEBUG
                    Console.WriteLine($"{GetType().Name}.GetList: {requestUri}");
#endif
                    return await Http.GetFromJsonAsync<List<ApplicationUser>>(requestUri);
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"{GetType().Name}.GetList: {ex.GetBaseException().Message}");
#endif
                throw;
            }
        }
        public async Task<ApplicationUser> Get(string id)
        {
            try
            {
                string requestUri = GetRequestUri(id);
#if DEBUG
                Console.WriteLine($"{GetType().Name}.Get: {requestUri}");
#endif
                return await Http.GetFromJsonAsync<ApplicationUser>(requestUri);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"{GetType().Name}.Get: id={id}; {ex.GetBaseException().Message}");
#endif
                throw;
            }
        }
        public async Task<ApplicationUser> AddNew(ApplicationUser newEntity)
        {
            try
            {
                string requestUri = GetRequestUri();
#if DEBUG
                Console.WriteLine($"{GetType().Name}.AddNew: {requestUri}");
#endif
                var result = await Http.PostAsJsonAsync(requestUri, newEntity);
                newEntity = await result.Content.ReadFromJsonAsync<ApplicationUser>();
                Reset();
                return newEntity;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"{GetType().Name}.AddNew: {ex.GetBaseException().Message}");
#endif
                throw;
            }
        }
        public async Task<ApplicationUser> Update(ApplicationUser entity)
        {
            try
            {
                string requestUri = GetRequestUri();
#if DEBUG
                Console.WriteLine($"{GetType().Name}.Update: {requestUri}");
#endif
                var result = await Http.PutAsJsonAsync(requestUri, entity);
                entity = await result.Content.ReadFromJsonAsync<ApplicationUser>();
                Reset();
                return entity;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"{GetType().Name}.Update: {ex.GetBaseException().Message}");
#endif
                throw;
            }
        }
        public async Task Delete(string id)
        {
            try
            {
                string requestUri = GetRequestUri(id);
#if DEBUG
                Console.WriteLine($"{GetType().Name}.Delete: {requestUri}");
#endif
                await Http.DeleteAsync(requestUri);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"{GetType().Name}.Delete: {ex.GetBaseException().Message}");
#endif
                throw;
            }
        }
        public void Reset()=> Cache.Remove(typeof(ApplicationUser));
    }
}