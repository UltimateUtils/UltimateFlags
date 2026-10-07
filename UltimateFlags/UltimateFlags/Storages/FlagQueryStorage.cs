using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Storages;

public class FlagQueryStorage : IFlagQueryStorage
{
    private readonly ILogger<FlagQueryStorage> _logger;

    private readonly IOptionsSnapshot<UltimateFlagConfiguration> _optionsSnapshot;

    public FlagQueryStorage(
        ILogger<FlagQueryStorage> logger,
        IOptionsSnapshot<UltimateFlagConfiguration> optionsSnapshot)
    {
        _logger = logger;
        _optionsSnapshot = optionsSnapshot;
    }

    public Flag? Read(Guid id, bool? deleted)
    {
        IEnumerable<Flag>? flags = _optionsSnapshot.Value.Flags;

        if (flags is null)
            return null;

        Dictionary<Guid, Flag> flagsMap = flags.ToDictionary(f => f.Id);

        if (deleted is null)
        {
            return flagsMap.GetValueOrDefault(id);
        }

        Flag? found = flagsMap.GetValueOrDefault(id);

        if (found is null)
            return null;

        if (deleted.Value && found.DeletedAt is not null
            || !deleted.Value && found.DeletedAt is null)
        {
            return found;
        }

        return null;

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
