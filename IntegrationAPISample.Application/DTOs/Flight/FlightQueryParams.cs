namespace IntegrationAPISample.Application.DTOs;

public class FlightQueryParams
{
    public string City { get; set; } = string.Empty;
    public string SpecificDate { get; set; } = string.Empty;
    public string DateTo { get; set; } = string.Empty;
    public int Passengers { get; set; }
}
