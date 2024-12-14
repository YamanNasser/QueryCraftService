using Microsoft.AspNetCore.Mvc;
using QueryCraftService.Models;
using QueryCraftService.Services.Interfaces;

namespace QueryCraftService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly IDatabaseService _databaseService;

        public ChatController(IOpenAIService openAIService, IDatabaseService databaseService)
        {
            _openAIService = openAIService;
            _databaseService = databaseService;
        }

        /// <summary>
        /// Generates an SQL query template based on user input and database schema.
        /// </summary>
        /// <param name="userInput">The user-provided query description.</param>
        /// <param name="tableName">The target database table name.</param>
        /// <returns>A formatted query template.</returns>
        private async Task<string> GenerateSQLQueryTemplate(string userInput, string tableName)
        {
            var fullQuery = await _databaseService.GetSchema(tableName);
            //you can update this template regard your business needs.
            return $@"
                ### Instructions ###
                You are an advanced SQL generator. Translate the following user request into a valid SQL query based on the provided database schema.

                ### Database Schema ###
                {fullQuery}

                ### User Request ###
                {userInput} 

                ### Rules ###
                1. Write a valid and optimized SQL query that satisfies the user request.
                2. Ensure the query adheres to SQL best practices (e.g., appropriate joins, filters, and grouping if applicable).
                3. Respond ONLY with the SQL query—no comments, explanations, or additional text.
                ";
        }

        /// <summary>
        /// Processes the user's query request and generates an SQL query using AI.
        /// </summary>
        /// <param name="request">The user's chat request.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>An SQL query response or appropriate error message.</returns>
        [HttpPost]
        [Route("query")]
        public async Task<ActionResult<ChatResponse>> ProcessQuery([FromBody] ChatRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.UserInput))
                return BadRequest("User input cannot be empty.");

            try
            {
                var template = await GenerateSQLQueryTemplate(request.UserInput, "apartments_data");
                var result = await _openAIService.ProcessQuery(request.UserInput, template);

                return Ok(result);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(499, new { message = "Request was cancelled." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
