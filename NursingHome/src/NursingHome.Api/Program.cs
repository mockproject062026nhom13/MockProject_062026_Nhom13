using DotNetEnv;
using NursingHome.Api.Middleware;
using NursingHome.Application;
using NursingHome.Infrastructure;

// Load variables from the nearest .env file (walking up from the working
// directory) into the process environment BEFORE the host is built.
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["ConnectionStrings:DefaultConnection"] =
        $"Server=127.0.0.1,14330;" +
        $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
        $"User Id={Environment.GetEnvironmentVariable("DB_APP_USER")};" +
        $"Password={Environment.GetEnvironmentVariable("DB_APP_PASSWORD")};" +
        "TrustServerCertificate=True;"
});

Console.WriteLine(Environment.GetEnvironmentVariable("DB_NAME"));
Console.WriteLine(Environment.GetEnvironmentVariable("DB_APP_USER"));
Console.WriteLine(Environment.GetEnvironmentVariable("DB_APP_PASSWORD"));

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();