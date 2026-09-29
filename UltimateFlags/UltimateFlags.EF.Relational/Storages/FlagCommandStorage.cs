using Microsoft.Extensions.Logging;
using UltimateFlags.EF.Db;

namespace UltimateFlags.EF.Relational.Storages;

public class FlagCommandStorage : EF.Storages.FlagCommandStorage
{
    public FlagCommandStorage(
        ILogger<FlagCommandStorage> logger,
        IFlagDbContext flagDbContext)
        : base(
            logger,
            flagDbContext)
    {
    }
}
