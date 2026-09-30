using Microsoft.Extensions.Options;
using UltimateFlags.Api.v10.Config;
using UltimateFlags.Api.v10.Contracts;
using UltimateFlags.Api.v10.Services.Abstraction;

namespace UltimateFlags.Api.v10.Services;

public class HealthCheckService : IHealthCheckService
{
    private readonly ILogger<HealthCheckService> _logger;

    private readonly ServiceConfiguration _serviceConfiguration;

    public HealthCheckService(
        ILogger<HealthCheckService> logger,
        IOptions<ServiceConfiguration> options)
    {
        _logger = logger;
        _serviceConfiguration = options.Value;
    }

    public HealthCheckResponse Ping(string? name = null)
    {
        return new HealthCheckResponse
        {
            Message = $"Pong, {name ?? "there"} (from {_serviceConfiguration.ServiceName})",
        };
    }
}
