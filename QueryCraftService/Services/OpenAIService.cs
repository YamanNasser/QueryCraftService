using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using QueryCraftService.Services.Interfaces;
using System.ClientModel;

namespace QueryCraftService.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly OpenAISettings _settings;
        private readonly IDatabaseService _databaseService;

        public OpenAIService(IOptions<OpenAISettings> settings, IDatabaseService databaseService)
        {
            _settings = settings.Value;
            _databaseService = databaseService;
        }

        public async Task<List<object>> ProcessQuery(string userInput, string template)
        {
            try
            {
                var client = InitializeChatClient();
                var result = await client.CompleteAsync(template);
                var sql = result.Choices[0].Text;

                if (sql != null && sql.Contains("SELECT"))
                {
                    sql = ExtractSelectQuery(sql);
                    var queryResult = await _databaseService.RunQuery(sql);
                    return GetQueryResult(queryResult);
                }
            }
            catch (Exception)
            {
                return [];//add log when some exception occure.
            }
            return [];
        }

        private IChatClient InitializeChatClient()
        {

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri(_settings.Endpoint)
            };

            var apiKey = _settings.ApiKey;
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("API key is not set.");

            var client = new OpenAIClient(new ApiKeyCredential(apiKey), options)
                .AsChatClient(_settings.Model);

            return client;
        }

        private string ExtractSelectQuery(string sql)
        {
            if (sql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) != 0)
                sql = sql.Substring(sql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase));

            return sql.Replace("```", "").Trim();
        }

        private List<object> GetQueryResult(IEnumerable<object> queryResult)
        {
            List<object> result = [];
            foreach (var item in queryResult)
            {
                result.Add(item);
            }
            return result;
        }
    }
}
