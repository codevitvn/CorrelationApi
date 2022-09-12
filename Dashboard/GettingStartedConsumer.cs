using MassTransit;
using MassTransitContracts;
using Microsoft.Extensions.Logging;
using LogContext = Serilog.Context.LogContext;

namespace Dashboard;

public class GettingStartedConsumer : IConsumer<HelloMessage>
{
    private readonly ILogger<GettingStartedConsumer> _logger;

    public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<HelloMessage> context)
    {
        LogContext.PushProperty("CorrelationId", context.Message.CorrelationId);
        
        _logger.LogInformation("Hello {Name}", context.Message.Name);
        
        return Task.CompletedTask;
    }
}