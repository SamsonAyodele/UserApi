using System.Net;
using System.Text.Json;
using UserApi.Helpers;

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
        var response = new ApiResponse<object>(false, exception.Message, null);
        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}