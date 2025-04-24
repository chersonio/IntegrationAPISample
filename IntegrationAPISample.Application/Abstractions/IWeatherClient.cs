using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Domain;

namespace IntegrationAPISample.Application.Abstractions;

public interface IWeatherClient
{
    Task<WeatherResponse?> GetWeatherAsync(WeatherQueryParams parameters, CancellationToken cancellationToken = default);
}
