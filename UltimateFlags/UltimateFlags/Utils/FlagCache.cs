using Microsoft.Extensions.Caching.Memory;

namespace UltimateFlags.Utils;

public class FlagCache
{
    public MemoryCache Cache { get; } =
        new(
            new MemoryCacheOptions
            {
                SizeLimit = 1024,
            });
}
