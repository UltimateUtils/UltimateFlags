using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.Managers;

namespace UltimateFlags.EF.Services.Singleton;

internal class FlagService : IFlagService
{
    private readonly ILogger<FlagService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagService(
        ILogger<FlagService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _ultimateFlagConfiguration = options.Value;
    }

    public bool IsOn(string key)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.IsOn(key);
    }
}
