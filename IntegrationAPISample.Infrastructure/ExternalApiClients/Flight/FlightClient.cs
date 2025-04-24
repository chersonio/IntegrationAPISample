using IntegrationAPISample.Application.Abstractions;
using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Application.Settings;
using IntegrationAPISample.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace IntegrationAPISample.Infrastructure.ExternalApis;

public class FlightClient : IFlightClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FlightClient> _logger;
    private readonly string _apiKey;

    public FlightClient(HttpClient httpClient, IOptions<FlightSettings> options, ILogger<FlightClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = options.Value.ApiKey;
    }

    //private List<FlightCity> _cities;
    private string _cities = "123";

    public async Task<FlightResponse?> GetFlightsAsync(FlightQueryParams parameters, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(parameters.City))
                throw new ArgumentException($"{parameters.City} is required", nameof(parameters.City));

            var cityUrl = $"https://api.aviationstack.com/v1/cities?access_key={_apiKey}";

            _logger.LogTrace("Requesting Flight data from: {Url}", cityUrl);
            


            if (_cities == null)
            {
                //var citiesResponse = await _httpClient.GetFromJsonAsync<FlightResponse>(cityUrl, cancellationToken);
                //_cities = citiesResponse;
            }


            return new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching weather data from {nameof(FlightClient)} for {parameters.City}");
            return null;
        }
    }
}
