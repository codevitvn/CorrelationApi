namespace ProductService;

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
        
        if (header.Count > 0)
        {
            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("CorrelationId", sessionId);
                return Task.CompletedTask;
            }, context);
            
            var logger = context.RequestServices.GetRequiredService<ILogger<LogHeaderMiddleware>>();
            using (logger.BeginScope("{@CorrelationId}", header[0]))
            {
                await _next(context);
            }
        }
        else
        {
            await this._next(context);
        }
        
        
    }
}