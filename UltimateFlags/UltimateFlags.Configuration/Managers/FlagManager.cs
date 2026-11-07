using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Helpers;

namespace UltimateFlags.Configuration.Managers;

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
        string[] ancestorKeys = [.. key.GetAncestorKeys(inclusive: true)];

        bool[] isOns =
        [
            .. _flagQueryStorage
                .ReadAll(ancestorKeys)
                /*.Where(x => x.IsOn)*/ // todo - performance check
                .Select(x => x.IsOn)
        ];

        return isOns.Length == ancestorKeys.Length
            && isOns.All(isOn => isOn);
    }
}
