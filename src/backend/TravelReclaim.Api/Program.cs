using Microsoft.EntityFrameworkCore;
using TravelReclaim.Api.Middleware;
using TravelReclaim.Application;
using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Invoices.Commands;
using TravelReclaim.Application.Invoices.Queries;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Infrastructure;
using TravelReclaim.Infrastructure.Persistence.SqlServer.Repositories;

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

// Infrastructure
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Repositories
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// CQRS Handlers - Commands
builder.Services.AddScoped<ICommandHandler<CreateInvoiceCommand, InvoiceResponse>, CreateInvoiceHandler>();

// CQRS Handlers - Queries
builder.Services.AddScoped<IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse>, GetInvoiceByIdHandler>();
builder.Services.AddScoped<IQueryHandler<GetInvoicesPagedQuery, PagedResponse<InvoiceResponse>>, GetInvoicesPagedHandler>();

var app = builder.Build();

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
