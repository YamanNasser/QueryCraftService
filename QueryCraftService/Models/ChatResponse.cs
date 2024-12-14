namespace QueryCraftService.Models
{
    public class ChatResponse
    {
        /// <summary>
        /// The generated SQL query based on the user input.
        /// </summary>
        public string SqlQuery { get; set; }

        /// <summary>
        /// The result of executing the SQL query against the database.
        /// </summary>
        public List<string> QueryResult { get; set; } = new List<string>();
    }
}
