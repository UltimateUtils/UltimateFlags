using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.Managers;
using UltimateFlags.Utils;

namespace UltimateFlags.EF.Services;

internal class FlagCacheService : IFlagCacheService
{
    private readonly ILogger<FlagCacheService> _logger;

    private readonly IFlagManager _flagManager;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    private readonly FlagCache _flagCache;

    private readonly MemoryCacheEntryOptions _flagCacheEntryOptions;

    public FlagCacheService(
        ILogger<FlagCacheService> logger,
        IFlagManager flagManager,
        IOptions<UltimateFlagConfiguration> options,
        FlagCache flagCache)
    {
        _logger = logger;
        _flagManager = flagManager;
        _ultimateFlagConfiguration = options.Value;
        _flagCache = flagCache;

        _flagCacheEntryOptions =
            new MemoryCacheEntryOptions
            {
                Size = 1,
                // SlidingExpiration = TimeSpan.FromSeconds(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_ultimateFlagConfiguration.CacheTimeLimitInSec),
            };
    }

    public bool IsOn(string key)
    {
        return
            _flagCache.Cache
                .GetOrCreate(
                    key,
                    _ => _flagManager.IsOn(key),
                    _flagCacheEntryOptions);
    }

    public void Evict(string key)
    {
        _flagCache.Cache.Remove(key);
    }

    public void Clear()
    {
        _flagCache.Cache.Clear();
    }

    public Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default)
    {
        return
            _flagCache.Cache
                .GetOrCreateAsync(
                    key,
                    _ => _flagManager.IsOnAsync(key, cancellationToken),
                    _flagCacheEntryOptions);
    }
}
