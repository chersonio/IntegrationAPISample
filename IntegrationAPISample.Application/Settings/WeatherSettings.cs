namespace IntegrationAPISample.Application.Settings;

public class WeatherSettings : IExternalApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}
