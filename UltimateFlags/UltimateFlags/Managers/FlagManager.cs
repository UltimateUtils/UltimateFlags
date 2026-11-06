using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Storages;

namespace UltimateFlags.Managers;

internal class FlagManager : IFlagManager
{
    private readonly ILogger<FlagManager> _logger;

    private readonly IFlagQueryStorage _flagQueryStorage;

    private readonly IFlagCommandStorage _flagCommandStorage;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagManager(
        ILogger<FlagManager> logger,
        IFlagQueryStorage flagQueryStorage,
        IFlagCommandStorage flagCommandStorage,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _flagQueryStorage = flagQueryStorage;
        _flagCommandStorage = flagCommandStorage;
        _ultimateFlagConfiguration = options.Value;
    }

    public bool IsOn(string key)
    {
        throw new NotImplementedException();
    }
}
