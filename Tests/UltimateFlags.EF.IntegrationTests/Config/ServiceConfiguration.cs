namespace UltimateFlags.EF.IntegrationTests.Config;

public record ServiceConfiguration
{
    public const string SectionName = "ServiceConfiguration";

    public const string DbFileNameKey = "FlagsDb";

    public required string ServiceName { get; init; }

    public string? DbFileName { get; init; }
}
