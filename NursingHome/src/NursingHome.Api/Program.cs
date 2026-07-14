using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Api.Middleware;
using NursingHome.Application;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.Repositories;

// Load variables from the nearest .env file (walking up from the working
// directory) into the process environment BEFORE the host is built, so they are
// picked up by builder.Configuration's environment-variable provider.
// In containers the values already come from the process environment and no
// .env file is present — TraversePath().Load() simply finds nothing and is a no-op.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Swagger / OpenAPI via Swashbuckle — https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    
builder.Services.AddDbContext<NursingHomeDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IBedRepository, BedRepository>();

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

app.UseFastEndpoints();

app.MapGet("/", () => "API Running");

app.Run();
