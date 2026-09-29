using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;

namespace UltimateFlags.EF.Relational.Managers;

public class FlagManager : EF.Managers.FlagManager
{
    private readonly IFlagQueryStorage _flagQueryStorage;

    public FlagManager(
        ILogger<FlagManager> logger,
        IFlagQueryStorage flagQueryStorage,
        IFlagCommandStorage flagCommandStorage,
        IOptions<UltimateFlagConfiguration> options)
        : base(
            logger,
            flagQueryStorage,
            flagCommandStorage,
            options)
    {
        _flagQueryStorage = flagQueryStorage;
    }

    public override bool IsOn(string key)
    {
        List<Flag> flags = [.. _flagQueryStorage.ReadAllAncestors(key)];
        return flags.All(f => f.IsOn);
    }
}
