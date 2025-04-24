using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Domain;

namespace IntegrationAPISample.Application.Mappers;

public static class WeatherMapper
{
    public static WeatherDto MapToDto(this WeatherResponse response)
    {
        return new WeatherDto
        {
            Weather = response.Weather,
            Temperature = response.Main.Temperature,
            FeelsLike = response.Main.FeelsLike,
            TemperatureMin = response.Main.TemperatureMin,
            TemperatureMax = response.Main.TemperatureMax,
            Wind = response.Wind
        };
    }
}

