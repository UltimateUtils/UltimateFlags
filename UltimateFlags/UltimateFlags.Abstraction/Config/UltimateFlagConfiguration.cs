namespace UltimateFlags.Abstraction.Config;

public record UltimateFlagConfiguration
{
    public const string SectionName = "UltimateFlags";

    public Dictionary<string, FlagValue>? Flags { get; set; }
}
