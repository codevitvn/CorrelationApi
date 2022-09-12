namespace CompanyService;

public class TargetClient
{
    private readonly HttpClient _httpClient;

    public TargetClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SampleAsync()
    {
        var response = await _httpClient.GetAsync("WeatherForecast");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}