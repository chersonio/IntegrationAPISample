using IntegrationAPISample.Application.Abstractions;
using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Application.Services;
using IntegrationAPISample.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntegrationAPISample.Tests;

public class AggregationServiceTests
{
    [Fact]
    public async Task GetAggregatedDataAsync_ReturnsWeather_WhenWeatherApiSucceeds()
    {
        // Arrange
        var weatherMock = new Mock<IWeatherClient>();
        weatherMock.Setup(x => x.GetWeatherAsync(It.IsAny<WeatherQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WeatherResponse
            {
                Weather = new List<Weather> { new Weather { Description = "Sunny", Id = 1 } },
                Main = new Main { FeelsLike = 23, Temperature = 25 }
            });

        var hotelMock = new Mock<IHotelClient>();
        hotelMock.Setup(x => x.GetHotelsAsync(It.IsAny<HotelQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HotelResponse
            {
            });

        var flightMock = new Mock<IFlightClient>();
        flightMock.Setup(x => x.GetFlightsAsync(It.IsAny<FlightQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FlightResponse
            {
            });

        var service = new AggregationService(
            weatherMock.Object,
            flightMock.Object,
            hotelMock.Object,
            new Mock<ILogger<AggregationService>>().Object);

        // Act
        var result = await service.GetAggregatedDataAsync(new AggregationQueryParams());

        // Assert
        Assert.NotNull(result.WeatherData);
        Assert.Equal("Sunny", result.WeatherData.Weather.FirstOrDefault().Description);
    }
}

