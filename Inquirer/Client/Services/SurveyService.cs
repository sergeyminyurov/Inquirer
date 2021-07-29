using Inquirer.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Inquirer.Services
{
    public sealed class SurveyService : ISurveyService
    {
        private HttpClient Http { get; }
        private ILogger<DataService> Logger { get; }
        public SurveyService(HttpClient http, ILogger<DataService> logger)
        {
            Http = http;
            Logger = logger;
        }
        public async Task<SurveyAnswer> SetAnswer(Survey survey, Question question, QuestionAnswer answer, bool single)
        {
            try
            {
                string requestUri = $"api/{nameof(SurveyAnswer).ToLower()}/{survey.Id}/{question.Id}/{answer.Id}/{single}";
                Logger.LogDebug($"{nameof(SurveyService)}.{nameof(SetAnswer)}: {requestUri}");
                HttpRequestMessage httpRequest = new () 
                { 
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestUri, UriKind.Relative)
                };
                var result = await Http.SendAsync(httpRequest);
                var entity = await result.Content.ReadFromJsonAsync<SurveyAnswer>();
                return entity;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(SurveyService)}.{nameof(SetAnswer)}: {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task RemoveAnswer(Survey survey, QuestionAnswer answer)
        {
            try
            {
                string requestUri = $"api/{nameof(SurveyAnswer).ToLower()}/{survey.Id}/{answer.Id}";
                Logger.LogDebug($"{nameof(SurveyService)}.{nameof(RemoveAnswer)}: {requestUri}");
                await Http.DeleteAsync(requestUri);
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(SurveyService)}.{nameof(RemoveAnswer)}: {ex.GetBaseException().Message}");
                throw;
            }
        }
    }
}