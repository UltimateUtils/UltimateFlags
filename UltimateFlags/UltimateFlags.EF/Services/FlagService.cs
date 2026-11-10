using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.Managers;

namespace UltimateFlags.EF.Services;

internal class FlagService : IFlagService
{
    private readonly ILogger<FlagService> _logger;

    private readonly IFlagManager _flagManager;

    public FlagService(
        ILogger<FlagService> logger,
        IFlagManager flagManager)
    {
        _logger = logger;
        _flagManager = flagManager;
    }

    public bool IsOn(string key)
    {
        return _flagManager.IsOn(key);
    }
}
