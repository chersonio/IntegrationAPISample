using IntegrationAPISample.Application.DTOs;

namespace IntegrationAPISample.Application.Abstractions;

public interface IAggregationService
{
    Task<AggregationResultDto> GetAggregatedDataAsync(AggregationQueryParams parameters, CancellationToken cancellationToken = default);
}
