using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Config;

internal static class FlagsConfigExtensions
{
    public static FlagsComplex GetFlagsComplex(this IEnumerable<Flag>? flags)
    {
        if (flags is null)
        {
            return
                new FlagsComplex
                {
                    All = [],
                    ByKey = new Dictionary<string, Flag>(),
                    ById = new Dictionary<Guid, Flag>(),
                };
        }

        Flag[] flagsArray = [.. flags];

        return
            new FlagsComplex
            {
                All = flagsArray,
                ByKey = flagsArray.ToDictionary(f => f.Key),
                ById = flagsArray.ToDictionary(f => f.Id),
            };
    }
}
