using DotNetEnv;
using NursingHome.Infrastructure;

Env.Load("../../.env");
Console.WriteLine($"DB_NAME = {Environment.GetEnvironmentVariable("DB_NAME")}");
Console.WriteLine($"DB_APP_USER = {Environment.GetEnvironmentVariable("DB_APP_USER")}");
Console.WriteLine($"DB_APP_PASSWORD = {Environment.GetEnvironmentVariable("DB_APP_PASSWORD")}");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["ConnectionStrings:DefaultConnection"] =
        $"Server=127.0.0.1;" +
        $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
        $"User Id={Environment.GetEnvironmentVariable("DB_APP_USER")};" +
        $"Password={Environment.GetEnvironmentVariable("DB_APP_PASSWORD")};" +
        "TrustServerCertificate=True;"
});

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();