namespace UltimateFlags.Api.Config;

public record ServiceConfiguration
{
    public const string SectionName = "ServiceConfiguration";

    public required string ServiceName { get; set; }
}
