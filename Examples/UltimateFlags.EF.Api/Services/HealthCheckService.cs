using Microsoft.Extensions.Options;
using UltimateFlags.EF.Api.Config;
using UltimateFlags.EF.Api.Contracts;
using UltimateFlags.EF.Api.Services.Abstraction;

namespace UltimateFlags.EF.Api.Services;

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
