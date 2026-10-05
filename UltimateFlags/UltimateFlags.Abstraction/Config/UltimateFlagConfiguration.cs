using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Abstraction.Config;

public record UltimateFlagConfiguration
{
    public const string SectionName = "UltimateFlags";

    public IEnumerable<Flag>? Flags { get; set; }
}
