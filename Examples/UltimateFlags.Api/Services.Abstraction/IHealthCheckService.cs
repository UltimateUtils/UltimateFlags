using UltimateFlags.Api.v10.Contracts;

namespace UltimateFlags.Api.v10.Services.Abstraction;

public interface IHealthCheckService
{
    public HealthCheckResponse Ping(string? name = null);
}
