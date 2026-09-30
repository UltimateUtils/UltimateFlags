using UltimateFlags.Api.Contracts;

namespace UltimateFlags.Api.Services.Abstraction;

public interface IHealthCheckService
{
    public HealthCheckResponse Ping(string? name = null);
}
