namespace QueryCraftService.Models
{
    public class ChatRequest
    {
        /// <summary>
        /// The user input or request that should be translated into a SQL query.
        /// </summary>
        public string UserInput { get; set; }
    }
}
