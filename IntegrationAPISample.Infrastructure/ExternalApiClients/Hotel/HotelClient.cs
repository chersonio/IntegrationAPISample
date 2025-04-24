using IntegrationAPISample.Application.Abstractions;
using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Application.Settings;
using IntegrationAPISample.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace IntegrationAPISample.Infrastructure.ExternalApis;

public class HotelClient : IHotelClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HotelClient> _logger;
    private readonly string _apiKey;

    public HotelClient(HttpClient httpClient, IOptions<HotelSettings> options, ILogger<HotelClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = options.Value.ApiKey ?? throw new ArgumentNullException(nameof(options.Value.ApiKey), "Hotel API key must be configured.");
    }

    public async Task<HotelResponse?> GetHotelsAsync(HotelQueryParams parameters, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(parameters.City))
                throw new ArgumentException($"{parameters.City} is required", nameof(parameters.City));

            var url = $"lookup.json?query={parameters.City}&lang=en&lookFor=both&limit=1&token={_apiKey}";

            _logger.LogTrace("Requesting Hotel data from: {Url}", url);

            return await _httpClient.GetFromJsonAsync<HotelResponse>(url, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching weather data from {nameof(HotelClient)} for {parameters.City}, {parameters.CountryCode}");
            return null;
        }
    }
}
