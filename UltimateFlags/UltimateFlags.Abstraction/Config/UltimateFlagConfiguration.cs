using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Abstraction.Config;

public record UltimateFlagConfiguration
{
    public const string SectionName = "UltimateFlags";

    public int CacheTimeLimitInSec { get; init; } = 60;

    public IEnumerable<Flag>? Flags { get; init; }
}
