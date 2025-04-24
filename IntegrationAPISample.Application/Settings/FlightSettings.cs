namespace IntegrationAPISample.Application.Settings;

public class FlightSettings : IExternalApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}
