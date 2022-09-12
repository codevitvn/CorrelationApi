using Serilog.Context;

namespace CompanyService;

public class LogHeaderMiddleware
{
    private readonly RequestDelegate _next;
    public LogHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var header = context.Request.Headers["CorrelationId"];
        var sessionId = header.Count > 0 ? header[0] : Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = sessionId;
        LogContext.PushProperty("CorrelationId", sessionId);
        context.Response.OnStarting(state =>
        {
            var httpContext = (HttpContext)state;
            httpContext.Response.Headers.Add("CorrelationId", sessionId);
            return Task.CompletedTask;
        }, context);
        await _next(context);
    }
}