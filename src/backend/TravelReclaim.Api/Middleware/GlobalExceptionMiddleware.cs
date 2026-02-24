using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelReclaim.Application.Exceptions;

namespace TravelReclaim.Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = exception switch
        {
            EntityNotFoundException notFound => new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "Entity Not Found",
                Detail = notFound.Message,
                Type = "https://tools.ietf.org/html/rfc7807"
            },
            BusinessRuleException businessRule => new ProblemDetails
            {
                Status = (int)HttpStatusCode.UnprocessableEntity,
                Title = "Business Rule Violation",
                Detail = businessRule.Message,
                Type = "https://tools.ietf.org/html/rfc7807"
            },
            InvalidOperationException invalidOp => new ProblemDetails
            {
                Status = (int)HttpStatusCode.Conflict,
                Title = "Invalid Operation",
                Detail = invalidOp.Message,
                Type = "https://tools.ietf.org/html/rfc7807"

            },
            _ => CreateInternalError(context, exception)
        };

        if (problemDetails.Status >= 500)
            logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        else
            logger.LogWarning("Handled exception: {ExceptionType} - {Message}", exception.GetType().Name, exception.Message);

        context.Response.StatusCode = problemDetails.Status ?? 500;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private ProblemDetails CreateInternalError(HttpContext context, Exception exception)
    {
        var env = context.RequestServices.GetRequiredService<IHostEnvironment>();
        return new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Internal Server Error",
            Detail = env.IsDevelopment() ? exception.ToString() : "An unexpected error occured",
            Type = "https://tools.ietf.org/html/rfc7807"
        };
    }
}