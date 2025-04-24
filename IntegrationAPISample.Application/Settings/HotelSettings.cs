namespace IntegrationAPISample.Application.Settings;

public class HotelSettings : IExternalApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}
