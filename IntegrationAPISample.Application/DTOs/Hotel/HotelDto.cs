using IntegrationAPISample.Domain;

namespace IntegrationAPISample.Application.DTOs;

public class HotelDto
{
    public Results Results { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Error { get; set; } = string.Empty;
}