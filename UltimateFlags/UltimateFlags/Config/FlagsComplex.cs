using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Config;

public record FlagsComplex
{
    public required Flag[] All { get; init; }

    public required Dictionary<string, Flag> ByKey { get; init; }

    public required Dictionary<Guid, Flag> ById { get; init; }
}
