using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.UserSecurity.Commands;
using NursingHome.Application.Features.UserSecurity.Validators;
using NursingHome.Infrastructure.Persistence.DbContexts;

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

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
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

//app.UseFastEndpoints();

app.MapGet("/", () => "API Running");

app.Run();