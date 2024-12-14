using QueryCraftService.Services;
using QueryCraftService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Services registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services and configuration
builder.Services.Configure<OpenAISettings>(configuration.GetSection("OpenAI"));
builder.Services.Configure<DatabaseSettings>(configuration.GetSection("Database"));
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IOpenAIService, OpenAIService>();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
