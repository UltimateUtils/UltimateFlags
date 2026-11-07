namespace UltimateFlags.Configuration.Api.Contracts;

public record HealthCheckResponse
{
    public required string Message { get; init; }
}
