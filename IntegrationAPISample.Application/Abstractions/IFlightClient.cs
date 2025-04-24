using IntegrationAPISample.Application.DTOs;
using IntegrationAPISample.Domain;

namespace IntegrationAPISample.Application.Abstractions;

public interface IFlightClient
{
    Task<FlightResponse> GetFlightsAsync(FlightQueryParams flightQueryResponse, CancellationToken cancellationToken);
}
