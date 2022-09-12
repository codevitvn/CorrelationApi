namespace CompanyService;

public class RequestHandler : DelegatingHandler
{
    private readonly ICorrelationIdAccessor _correlationIdAccessor;

    public RequestHandler(ICorrelationIdAccessor correlationIdAccessor)
    {
        _correlationIdAccessor =
            correlationIdAccessor ?? throw new ArgumentNullException(nameof(correlationIdAccessor));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("CorrelationId", _correlationIdAccessor.GetCorrelationId());
        
        // Getting correlationid from request context.
        return base.SendAsync(request, cancellationToken);
    }
}