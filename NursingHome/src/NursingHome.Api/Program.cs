<<<<<<< HEAD
using DotNetEnv;
using NursingHome.Infrastructure;

Env.Load("../../.env");
Console.WriteLine($"DB_NAME = {Environment.GetEnvironmentVariable("DB_NAME")}");
Console.WriteLine($"DB_APP_USER = {Environment.GetEnvironmentVariable("DB_APP_USER")}");
Console.WriteLine($"DB_APP_PASSWORD = {Environment.GetEnvironmentVariable("DB_APP_PASSWORD")}");
=======
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Repositories.UserSecurity;
using NursingHome.Application.Features.UserSecurity.Commands;
using NursingHome.Application.Features.UserSecurity.Validators;
using NursingHome.Infrastructure.Persistence.DbContexts;

using DotNetEnv;
using NursingHome.Api.Middleware;
using NursingHome.Application;

// Load variables from the nearest .env file (walking up from the working
// directory) into the process environment BEFORE the host is built, so they are
// picked up by builder.Configuration's environment-variable provider.
// In containers the values already come from the process environment and no
// .env file is present — TraversePath().Load() simply finds nothing and is a no-op.
Env.TraversePath().Load();
>>>>>>> dev

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

builder.Services.AddDbContext<NursingHomeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddApplication();

var app = builder.Build();

<<<<<<< HEAD
=======
// Configure the HTTP request pipeline.

// Register first so it wraps every downstream middleware and endpoint, turning
// unhandled exceptions (incl. FluentValidation's ValidationException) into ApiResponse errors.
app.UseMiddleware<ExceptionHandlingMiddleware>();

>>>>>>> dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();