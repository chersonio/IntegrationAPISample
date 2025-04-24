namespace IntegrationAPISample.Application.DTOs;

public class WeatherQueryParams
{
    public string City { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;
    public string? DaysAhead { get; set; }
}