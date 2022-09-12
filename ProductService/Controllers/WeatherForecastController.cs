using MassTransit;
using MassTransitContracts;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ICorrelationIdAccessor _correlationIdAccessor;
    private readonly IBus _bus;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IBus bus, ICorrelationIdAccessor correlationIdAccessor)
    {
        _logger = logger;
        _bus = bus;
        _correlationIdAccessor = correlationIdAccessor;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        _logger.LogInformation("Target method invoked.");
        await _bus.Publish(new HelloMessage()
        {
            Name = "Target",
            CorrelationId = _correlationIdAccessor.GetCorrelationId()
        });
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
