using Microsoft.AspNetCore.Mvc;

namespace CompanyService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly TargetClient _targetClient;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, TargetClient targetClient)
    {
        _logger = logger;
        _targetClient = targetClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        // _logger.Verbose("This is a Verbose log message");
        // _logger.Debug("This is a Debug log message");
        // _logger.Information("This is an Information log message");
        // _logger.Warning("This is a Warning log message");
        // _logger.Error("This is a Error log message");
        // _logger.Fatal("This is a Fatal log message");
        _logger.LogTrace("This is a Verbose log message");
        _logger.LogDebug("This is a Debug log message");
        _logger.LogInformation("This is an Information log message");
        _logger.LogWarning("This is a Warning log message");
        _logger.LogError("This is a Error log message");
        _logger.LogCritical("This is a Fatal log message");
        _logger.LogInformation("Weather Forecast executing...");
        var response =  await _targetClient.SampleAsync();
        _logger.LogInformation(response);
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}