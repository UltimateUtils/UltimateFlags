using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Configuration.Managers;

namespace UltimateFlags.Configuration.Services.Singleton;

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

        return flagManager.IsOn(key);
    }

    public Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotSupported { Area = $"{nameof(FlagService)}.{nameof(IsOnAsync)}(key)", };
    }
}
