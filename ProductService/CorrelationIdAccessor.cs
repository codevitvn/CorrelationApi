namespace ProductService;

public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    private readonly ILogger<CorrelationIdAccessor> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdAccessor(ILogger<CorrelationIdAccessor> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public string GetCorrelationId()
    {
        try
        {
            var context = this._httpContextAccessor.HttpContext;
            var result = context?.Items["CorrelationId"] as string;

            return result;
        }
        catch (Exception exception)
        {
            this._logger.LogWarning(exception, "Unable to get correlation id in header");
        }

        return string.Empty;
    }
}