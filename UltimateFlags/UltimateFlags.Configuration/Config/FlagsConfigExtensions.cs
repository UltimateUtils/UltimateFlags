using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Configuration.Config;

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
                    ByName = new Dictionary<(string, Guid?), Flag>(),
                };
        }

        return _GetFlagsComplex(flags);
    }

    private static FlagsComplex _GetFlagsComplex(IEnumerable<Flag> flags)
    {
        Flag[] allFlags = [.. flags];

        Dictionary<string, Flag> byKey = allFlags.ToDictionary(f => f.Key);
        Dictionary<Guid, Flag> byId = allFlags.ToDictionary(f => f.Id);
        Dictionary<(string, Guid?), Flag> byName = allFlags.ToDictionary(f => (f.Name, f.ParentId));

        foreach (Flag flag in allFlags)
        {
            if (flag.ParentId is not null)
            {
                flag.Parent = byId[flag.ParentId.Value];

                flag.Parent.Children ??= new List<Flag>();
                flag.Parent.Children.Add(flag);
            }
        }

        return
            new FlagsComplex
            {
                All = allFlags,
                ByKey = byKey,
                ById = byId,
                ByName = byName,
            };
    }
}
