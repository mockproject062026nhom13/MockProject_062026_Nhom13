using Microsoft.EntityFrameworkCore;
using FluentValidation;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.Repositories.AdmissionEhrCarePlan;
using NursingHome.Infrastructure.Persistence.Repositories.LocationInfrastructure;
using NursingHome.Infrastructure.Persistence.Repositories.Rbac;
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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Swagger / OpenAPI via Swashbuckle — https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NursingHomeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRbacRepository, RbacRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IAdmissionRepository, AdmissionRepository>();

builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Register first so it wraps every downstream middleware and endpoint, turning
// unhandled exceptions (incl. FluentValidation's ValidationException) into ApiResponse errors.
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
