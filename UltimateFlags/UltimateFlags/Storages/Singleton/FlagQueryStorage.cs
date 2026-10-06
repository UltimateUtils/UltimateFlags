using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Storages.Singleton;

public class FlagQueryStorage : IFlagQueryStorage
{
    private readonly ILogger<FlagQueryStorage> _logger;

    private readonly IOptionsMonitor<UltimateFlagConfiguration> _optionsSnapshot;

    public FlagQueryStorage(
        ILogger<FlagQueryStorage> logger,
        IOptionsMonitor<UltimateFlagConfiguration> optionsSnapshot)
    {
        _logger = logger;
        _optionsSnapshot = optionsSnapshot;
    }

    public Flag? Read(Guid id, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public Flag? Read(string key)
    {
        throw new NotImplementedException();
    }

    public Flag? Read(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Flag> ReadAllAncestors(string key, bool inclusive)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Flag> ReadAll(Guid? parentId, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Flag> ReadAllDeleted(DateTime? fromInclusive, DateTime? toInclusive)
    {
        throw new NotImplementedException();
    }

    public IPagedList<Flag> List(string? searchString, bool? isOn, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public bool Exists(Guid id, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public bool Exists(string name, Guid? parentId, bool? deleted)
    {
        throw new NotImplementedException();
    }
}
