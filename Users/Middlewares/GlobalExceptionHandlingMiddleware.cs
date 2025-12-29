
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Users.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger,IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;
            _logger.LogError(ex, "Unhandled exception (TraceId: {TraceId})", traceId);

            ProblemDetails problem;
            if (ex is UserException appEx) 
            {
                context.Response.StatusCode = appEx.StatusCode;
                problem = new ProblemDetails
                {
                    Status = appEx.StatusCode,
                    Title = appEx.Message,
                    Type = ex.GetType().Name
                };
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problem = new ProblemDetails
                {
                    Status = 500,
                    Title = "Error interno en el servidor",
                    Detail = _env.IsDevelopment() ? ex.Message : null
                };
            }

            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }

    }
}
