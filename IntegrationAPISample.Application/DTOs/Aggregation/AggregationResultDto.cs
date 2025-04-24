namespace IntegrationAPISample.Application.DTOs
{
    public class AggregationResultDto
    {
        public WeatherDto? WeatherData { get; set; }
        
        public FlightDto? FlightData { get; set; }
        
        public HotelDto? HotelData { get; set; }
    }
}
