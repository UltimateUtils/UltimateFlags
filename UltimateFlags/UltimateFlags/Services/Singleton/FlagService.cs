using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Managers;

namespace UltimateFlags.Services.Singleton;

internal class FlagService : IFlagService
{
    private readonly ILogger<FlagService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public FlagService(
        ILogger<FlagService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public bool IsOn(string key)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }
}
