using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Domain;

namespace IntegrationAPISample.Application.Abstractions;

public interface IHotelClient
{
    Task<HotelResponse> GetHotelsAsync(HotelQueryParams parameters, CancellationToken cancellationToken);
}
