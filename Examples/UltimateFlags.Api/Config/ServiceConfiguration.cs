namespace UltimateFlags.Api.v10.Config;

public record ServiceConfiguration
{
    public const string SectionName = "ServiceConfiguration";

    public required string ServiceName { get; set; }
}
