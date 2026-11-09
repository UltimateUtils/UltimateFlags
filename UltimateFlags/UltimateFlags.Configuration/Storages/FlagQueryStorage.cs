using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Configuration.Config;
using UltimatePagination;
using UltimatePagination.Abstraction;
using UltimateUtils.Extensions;

namespace UltimateFlags.Configuration.Storages;

public class FlagQueryStorage : IFlagQueryStorage
{
    private readonly ILogger<FlagQueryStorage> _logger;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    private readonly Flag[] _flags;

    private readonly FlagsComplex _flagsComplex;

    public FlagQueryStorage(
        ILogger<FlagQueryStorage> logger,
        IOptionsSnapshot<UltimateFlagConfiguration> optionsSnapshot)
    {
        _logger = logger;
        _ultimateFlagConfiguration = optionsSnapshot.Value;
        _flags = optionsSnapshot.Value.Flags?.ToArray() ?? [];
        _flagsComplex = optionsSnapshot.Value.Flags.GetFlagsComplex();
    }

    public Flag? Read(Guid id, bool? deleted = false, bool includeParent = false)
    {
        Flag? found = _flagsComplex.ById.GetValueOrDefault(id);

        if (found is null || deleted is null)
        {
            return found;
        }

        return
            deleted.Value == found.DeletedAt.HasValue
                ? found
                : null;
    }

    public Flag? Read(string name, Guid? parentId)
    {
        return _flagsComplex.ByName.GetValueOrDefault((name, parentId));
    }

    public Flag? Read(string key)
    {
        return _flagsComplex.ByKey.GetValueOrDefault(key);
    }

    public IQueryable<Flag> ReadAll(IEnumerable<string> keys)
    {
        return _readAll().AsQueryable();

        IEnumerable<Flag> _readAll()
        {
            foreach (string key in keys)
            {
                yield return _flagsComplex.ByKey[key];
            }
        }
    }

    public IQueryable<Flag> ReadAll(Guid? parentId, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Flag> ReadAllDeleted(DateTime? fromInclusive = null, DateTime? toInclusive = null)
    {
        return _readAll().AsQueryable();

        // todo - improve with index tree
        IEnumerable<Flag> _readAll()
        {
            foreach (Flag flag in _flagsComplex.All)
            {
                if (!flag.DeletedAt.HasValue)
                    continue;

                if ((fromInclusive is null || fromInclusive <= flag.DeletedAt.Value)
                    && (toInclusive is null || flag.DeletedAt.Value <= toInclusive))
                {
                    yield return flag;
                }
            }
        }
    }

    public IPagedList<Flag> List(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize)
    {
        return
            _flagsComplex
                .All
                .Where(
                    flag =>
                        (isOn == null || flag.IsOn == isOn)
                        && (searchString.IsNullOrEmpty()
                            || flag.Name.Contains(searchString)))
                .OrderBy(flag => flag.Key)
                .Paginate(pageNumber, pageSize);
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
