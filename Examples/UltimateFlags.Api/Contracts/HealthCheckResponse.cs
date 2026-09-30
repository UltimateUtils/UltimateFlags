namespace UltimateFlags.Api.v10.Contracts;

public record HealthCheckResponse
{
    public required string Message { get; init; }
}
