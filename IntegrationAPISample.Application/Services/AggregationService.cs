using IntegrationAPISample.Application.Abstractions;
using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Application.Mappers;
using Microsoft.Extensions.Logging;

namespace IntegrationAPISample.Application.Services;

public class AggregationService : IAggregationService
{
    private readonly IWeatherClient _weatherClient;
    private readonly IFlightClient _flightClient;
    private readonly IHotelClient _hotelClient;
    private readonly ILogger<AggregationService> _logger;

    public AggregationService(
        IWeatherClient weatherClient,
        IFlightClient flightClient,
        IHotelClient hotelClient,
        ILogger<AggregationService> logger)
    {
        _weatherClient = weatherClient;
        _flightClient = flightClient;
        _hotelClient = hotelClient;
        _logger = logger;
    }

    public async Task<AggregationResultDto> GetAggregatedDataAsync(AggregationQueryParams parameters, CancellationToken cancellationToken = default)
    {
        var weatherTask = Safe(() => _weatherClient.GetWeatherAsync(
            new WeatherQueryParams
            {
                City = parameters.City,
                CountryCode = parameters.CountryCode
            }, cancellationToken));

        var flightTask = Safe(() => _flightClient.GetFlightsAsync(
            new FlightQueryParams
            {
                City = parameters.City,
                SpecificDate = parameters.SpecificDate,
                Passengers = parameters.Passengers
            }, cancellationToken));

        var hotelTask = Safe(() => _hotelClient.GetHotelsAsync(
            new HotelQueryParams
            {
                City = parameters.City,
                CountryCode = parameters.CountryCode,
                SpecificDate = parameters.SpecificDate,
            }, cancellationToken));

        await Task.WhenAll(weatherTask, flightTask, hotelTask);

        var weatherData = (await weatherTask)?.MapToDto();
        var flightData = (await flightTask)?.MapToDto();
        var hotelData = (await hotelTask)?.MapToDto();

        return new AggregationResultDto
        {
            WeatherData = weatherData ?? new WeatherDto { Error = "Weather data unavailable" },
            FlightData = flightData ?? new FlightDto { Error = "Flight data unavailable" },
            HotelData = hotelData ?? new HotelDto { Error = "Hotel data unavailable" }
        };
    }

    private async Task<T> Safe<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, $"{func.Method.Name} failed due to transient issue.");

            return default;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, $"{func.Method.Name} request timed out.");

            return default;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Unexpected error from {func.Method.Name}");

            return default!;
        }
    }
}
