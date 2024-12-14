using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using QueryCraftService.Services.Interfaces;

namespace QueryCraftService.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseSettings _dbSettings;

        public DatabaseService(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        public async Task<List<string>> RunQuery(string query)
        {
            using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<object>(query);
            return result.Select(row => JsonConvert.SerializeObject(row)).ToList();
        }

        public async Task<string> GetSchema(string tableName)
        {
            using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);
            await connection.OpenAsync();
             
            var allTablesSql = await connection.QueryAsync<TableColumnInfo>(@$"
                SELECT table_name AS Table_Name, 
                       column_name AS Column_Name, 
                       data_type AS Data_Type, 
                       character_maximum_length AS Size, 
                       numeric_precision || '/' || numeric_scale AS Precision_Scale,
                       col_description(@tableName::regclass, ordinal_position) AS Column_description
                FROM information_schema.columns
                WHERE table_schema = 'public' AND table_name =@tableName
                ORDER BY table_name, ordinal_position;", new { tableName });

            var byTables = allTablesSql.GroupBy(t => t.Table_Name);

            return GetDescriptiveOfAllTables(byTables);
        }

        private string GetDescriptiveOfAllTables(IEnumerable<IGrouping<string, TableColumnInfo>> allTables)
        {
            string properties = string.Empty;

            foreach (var table in allTables)
            {
                properties += $"Entity name: {table.Key} has the following properties: ";

                foreach (var property in table)
                {
                    properties += $"{property.Column_Name} as {property.Data_Type}. ";
                }
            }

            return properties;
        }

        public class TableColumnInfo
        {
            public string Table_Name { get; set; }
            public string Column_Name { get; set; }
            public string Data_Type { get; set; }
            public int? Size { get; set; }
            public string Precision_Scale { get; set; }
            public string Column_description { get; set; }

        }
    }
}
