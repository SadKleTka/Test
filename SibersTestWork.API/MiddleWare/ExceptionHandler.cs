using System.Text.Json;
using SibersDataManager.Models.Exceptions;

namespace SibersTestWork.MiddleWare;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, ex.Message);
        
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = ex switch
        {
            BusinessValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var result = JsonSerializer.Serialize(new
        {
            message = ex.Message,
            Code = context.Response.StatusCode,
            DateTime = DateTime.UtcNow
        });

        return context.Response.WriteAsync(result);
    }
}