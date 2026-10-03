using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.Managers;

namespace UltimateFlags.EF.Services;

internal class FlagService : IFlagService
{
    private readonly ILogger<FlagService> _logger;

    private readonly IFlagManager _flagManager;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagService(
        ILogger<FlagService> logger,
        IFlagManager flagManager,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _flagManager = flagManager;
        _ultimateFlagConfiguration = options.Value;
    }

    public bool IsOn(string key)
    {
        return _flagManager.IsOn(key);
    }
}
