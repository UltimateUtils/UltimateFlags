using UltimateFlags.EF.Api.Contracts;

namespace UltimateFlags.EF.Api.Services.Abstraction;

public interface IHealthCheckService
{
    public HealthCheckResponse Ping(string? name = null);
}
