# QueryCraftService

## Introduction

**QueryCraftService** is a powerful backend service designed to generate SQL queries from natural language input. It leverages OpenAI's advanced language models to translate user requests into optimized SQL queries, which can then be executed against a PostgreSQL database. This service allows users to query the database in a more intuitive, conversational manner without having to manually write SQL queries.

Key features include:
- Dynamic SQL generation based on user input and database schema.
- Cuuretly only support PostgreSQL for executing queries.
- Utilizes OpenAI's models to ensure query accuracy and efficiency.

  ### **Note**: 
This project is not fully completed and is not intended for use in production environments. It is currently in the learning phase and is provided for educational and experimentation purposes only. The project may have unfinished features that need to be addressed before it can be used in any commercial or production environment.

## Development Requirements

To run and develop the **QueryCraftService**, you'll need the following tools and dependencies:

### Prerequisites
- **.NET 8 SDK**: Required for building and running the project. [Install .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
- **PostgreSQL**: This service connects to a PostgreSQL database for executing SQL queries. Ensure you have access to a PostgreSQL instance.
- **OpenAI API Key**: To interact with the OpenAI models, you must create an account on Groq and obtain an API key. [Sign up for Groq](https://www.groq.com).
- **Visual Studio Code / IDE of Choice**: Any IDE with support for .NET development, such as Visual Studio or Visual Studio Code.

### Configuration
You will need to configure the following settings in the appsettings.json:
- OpenAI Endpoint: The endpoint for Groq's OpenAI API.
- API Key: Your Groq API key.
- PostgreSQL Connection String: The connection string to your PostgreSQL instance.

# License
Distributed under the MIT License.

# Founders
 <a href="https://www.linkedin.com/in/yamannasser/">Yaman Nasser</a> Software Engineer, Palestine

# Support
Has this Project helped you learn something New? or Helped you at work? Do Consider Supporting. Here are a few ways by which you can support.
1. Leave a star! ⭐
2. Recommend this awesome project to your colleagues.
