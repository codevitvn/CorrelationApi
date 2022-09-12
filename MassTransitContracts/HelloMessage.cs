namespace MassTransitContracts;

public record HelloMessage
{
    public string CorrelationId { get; init; }
    public string Name { get; init; }
}