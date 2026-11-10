using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.Managers;

namespace UltimateFlags.EF.Services.Singleton;

internal class FlagService : IFlagService
{
    private readonly ILogger<FlagService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FlagService(
        ILogger<FlagService> logger,
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
}
