namespace IntegrationAPISample.Application.DTOs;

public class HotelQueryParams
{
    public string City { get; set; } = string.Empty;

    public string SpecificDate { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;
}
