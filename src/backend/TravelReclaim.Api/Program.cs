using Microsoft.EntityFrameworkCore;
using TravelReclaim.Api.Middleware;
using TravelReclaim.Application.Extensions;
using TravelReclaim.Infrastructure;
using TravelReclaim.Infrastructure.Extensions;
using TravelReclaim.Infrastructure.Persistence.MongoDB;

var builder = WebApplication.CreateBuilder(args);
var FRONTEND_POLICY = "AllowFrontend";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(FRONTEND_POLICY, policy =>
        policy.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Ensure MongoDB indexes (skip in testing environment)
var mongoContext = app.Services.GetService<MongoDbContext>();
if (mongoContext is not null)
    await MongoDbIndexSetup.EnsureIndexesAsync(mongoContext);

// --- Middleware Pipeline ---
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(FRONTEND_POLICY);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Make Program accesible to integration tests
public partial class Program { }