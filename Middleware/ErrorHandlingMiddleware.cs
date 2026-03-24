using System.Net;
using System.Text.Json;

namespace UserApi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new
        {
            success = false,
            message = "Something went wrong",
            error = exception.Message // ⚠️ remove in production later
        };
        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }   
}