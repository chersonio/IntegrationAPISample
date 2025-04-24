using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Domain;

namespace IntegrationAPISample.Application.Mappers;

public static class HotelMapper
{
    public static HotelDto MapToDto(this HotelResponse response)
    {
        return new HotelDto
        {
            Results = response.Results,
            Status = response.Status
        };
    }
}

