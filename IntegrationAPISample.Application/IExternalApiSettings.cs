namespace IntegrationAPISample.Application;

public interface IExternalApiSettings
{
    public string ApiKey { get; set; }
    public string Endpoint { get; set; } 
}