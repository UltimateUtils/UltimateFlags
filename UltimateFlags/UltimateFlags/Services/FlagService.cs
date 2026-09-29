using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Managers;
using UltimateFlags.Abstraction.Services;

namespace UltimateFlags.Services;

public class FlagService : IFlagService
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

    public bool IsOn(Guid id)
    {
        return _flagManager.IsOn(id);
    }

    public bool IsOn(string key)
    {
        return _flagManager.IsOn(key);
    }
}
