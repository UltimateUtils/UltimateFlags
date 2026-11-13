using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Configuration.Managers;

namespace UltimateFlags.Configuration.Services;

internal class FlagCacheService : IFlagCacheService
{
    private readonly ILogger<FlagCacheService> _logger;

    private readonly IFlagManager _flagManager;

    public FlagCacheService(
        ILogger<FlagCacheService> logger,
        IFlagManager flagManager)
    {
        _logger = logger;
        _flagManager = flagManager;
    }

    public bool IsOn(string key)
    {
        return _flagManager.IsOn(key);
    }

    public Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagCacheService)}.{nameof(IsOnAsync)}(key)", };
    }

    public void Evict(string key)
    {
        throw new NotSupported { Area = $"{nameof(FlagCacheService)}.{nameof(Evict)}(key)", };
    }

    public void Clear()
    {
        throw new NotSupported { Area = $"{nameof(FlagCacheService)}.{nameof(Clear)}()", };
    }
}
