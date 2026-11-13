using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.Managers;

namespace UltimateFlags.EF.Services.Singleton;

internal class FlagCacheService : IFlagCacheService
{
    private readonly ILogger<FlagCacheService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FlagCacheService(
        ILogger<FlagCacheService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public bool IsOn(string key)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.IsOn(key);
    }

    public Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.IsOnAsync(key, cancellationToken);
    }

    public void Evict(string key)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }
}
