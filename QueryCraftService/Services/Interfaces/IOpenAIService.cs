
namespace QueryCraftService.Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<List<object>> ProcessQuery(string userInput, string template);
    }
}
