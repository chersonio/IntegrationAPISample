using IntegrationAPISample.Application.Abstractions;
using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Application.Settings;
using IntegrationAPISample.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace IntegrationAPISample.Infrastructure.ExternalApis;

public class WeatherClient : IWeatherClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherClient> _logger;
    private readonly string _apiKey;

    public WeatherClient(HttpClient httpClient, IOptions<WeatherSettings> options, ILogger<WeatherClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = options.Value.ApiKey ?? throw new ArgumentNullException(nameof(options.Value.ApiKey), "Weather API key must be configured.");
    }

    public async Task<WeatherResponse?> GetWeatherAsync(WeatherQueryParams parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(parameters.City))
                throw new ArgumentException($"{parameters.City} is required", nameof(parameters.City));

            if (string.IsNullOrWhiteSpace(parameters.CountryCode))
                throw new ArgumentException($"{parameters.CountryCode} is required", nameof(parameters.CountryCode));

            var daysAhead = !string.IsNullOrWhiteSpace(parameters.DaysAhead) ? $"&cnt={parameters.DaysAhead}" : "";

            var url = $"data/2.5/weather?q={parameters.City},{parameters.CountryCode}{daysAhead}&units=metric&appid={_apiKey}";

            _logger.LogTrace("Requesting Hotel data from: {Url}", url);

            return await _httpClient.GetFromJsonAsync<WeatherResponse>(url, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching weather data from {nameof(WeatherClient)} for {parameters.City}, {parameters.CountryCode}");
            return null;
        }
    }
}
