namespace QueryCraftService.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<List<string>> RunQuery(string query);
        Task<string> GetSchema(string tableName);
    }
}
