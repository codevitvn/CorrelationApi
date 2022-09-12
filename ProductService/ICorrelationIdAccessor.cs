namespace ProductService;

public interface ICorrelationIdAccessor
{
    string GetCorrelationId();
}