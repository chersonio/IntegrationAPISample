using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;

namespace IntegrationAPISample.Infrastructure.BootsrapExtensions;

public static class HttpClientBuilderExtensions
{
    public static IServiceCollection AddResilientPolicy<TClient, TImplementation>(this IServiceCollection services)
        where TClient : class
        where TImplementation : class, TClient
    {
        services.AddHttpClient<TClient, TImplementation>()
               .AddPolicyHandler((serviceProvider, request) =>
               {
                   var logger = serviceProvider.GetRequiredService<ILogger<TImplementation>>();

                   return GetRetryPolicy(logger);
               });

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning(outcome.Exception,
                                            "Retry {RetryAttempt} after {SleepDuration} due to: {Message}",
                                            retryAttempt, timespan, outcome.Exception?.Message ?? "unknown error");
                });
    }
}
