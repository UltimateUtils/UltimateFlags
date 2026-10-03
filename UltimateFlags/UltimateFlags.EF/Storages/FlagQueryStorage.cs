using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.EF.Db;
using UltimatePagination;
using UltimatePagination.Abstraction;
using UltimateUtils.Extensions;

namespace UltimateFlags.EF.Storages;

public class FlagQueryStorage : IFlagQueryStorage
{
    private readonly ILogger<FlagQueryStorage> _logger;

    private readonly IFlagDbContext _flagDbContext;

    private readonly IQueryable<Flag> _flags;

    public FlagQueryStorage(
        ILogger<FlagQueryStorage> logger,
        IFlagDbContext flagDbContext)
    {
        _logger = logger;
        _flagDbContext = flagDbContext;
        _flags = flagDbContext.Flags.AsNoTracking();
    }

    public Flag? Read(Guid id, bool? deleted = false)
    {
        IQueryable<Flag> flagsQuery =
            _flagDbContext
                .Flags
                .AsNoTracking();

        return
            deleted is null
                ? flagsQuery
                    .IgnoreQueryFilters()
                    .FirstOrDefault(flag => flag.Id == id)
                : deleted.Value
                    ? flagsQuery
                        .IgnoreQueryFilters()
                        .FirstOrDefault(flag => flag.Id == id && flag.DeletedAt.HasValue)
                    : flagsQuery
                        .FirstOrDefault(flag => flag.Id == id);
    }

    public Flag? Read(string key)
    {
        return
            _flagDbContext
                .Flags
                .AsNoTracking()
                .FirstOrDefault(
                    flag =>
                        flag.Key == key);
    }

    public Flag? Read(string name, Guid? parentId)
    {
        return
            _flagDbContext
                .Flags
                .AsNoTracking()
                .FirstOrDefault(
                    flag =>
                        flag.Name == name
                        && flag.ParentId == parentId);
    }

    public IQueryable<Flag> ReadAllAncestors(string key, bool inclusive)
    {
        List<string> ancestorKeys = _getAncestorKeys(key, inclusive);

        return
            _flagDbContext
                .Flags
                .AsNoTracking()
                .Where(
                    flag =>
                        ancestorKeys.Contains(flag.Key)
                        && _flagDbContext.Flags.Any(leaf => leaf.Key == key));

        static List<string> _getAncestorKeys(string key, bool inclusive)
        {
            string[] names = key.Split(Utils.Constants.KeyDelimiter);

            IEnumerable<string> ancestorNames =
                inclusive
                    ? names.Select((_, index) => string.Join(Utils.Constants.KeyDelimiter, names[..(index + 1)]))
                    : names.Take(names.Length - 1).Select((_, index) => string.Join(Utils.Constants.KeyDelimiter, names[..(index + 1)]));

            return [.. ancestorNames];
        }
    }

    public IQueryable<Flag> ReadAll(Guid? parentId, bool? deleted = false)
    {
        IQueryable<Flag> flagsQuery = _flagDbContext.Flags.AsNoTracking();

        return
            deleted is null
                ? flagsQuery
                    .IgnoreQueryFilters()
                    .Where(flag => flag.ParentId == parentId)
                : deleted.Value
                    ? flagsQuery
                        .IgnoreQueryFilters()
                        .Where(flag => flag.ParentId == parentId && flag.DeletedAt.HasValue)
                    : flagsQuery
                        .Where(flag => flag.ParentId == parentId);
    }

    public IQueryable<Flag> ReadAllDeleted(DateTime? fromInclusive, DateTime? toInclusive)
    {
        return
            _flagDbContext
                .Flags
                .AsNoTracking()
                .Where(
                    flag =>
                        flag.DeletedAt.HasValue
                        && (fromInclusive == null || flag.DeletedAt.Value >= fromInclusive)
                        && (toInclusive == null || flag.DeletedAt.Value <= toInclusive));
    }

    public IPagedList<Flag> List(string? searchString, bool? isOn, int pageNumber, int pageSize)
    {
        return
            _flagDbContext
                .Flags
                .AsNoTracking()
                .Where(
                    flag =>
                        (isOn == null || flag.IsOn == isOn)
                        && (searchString.IsNullOrEmpty()
                            || flag.Name.Contains(searchString)))
                .OrderBy(flag => flag.Name) // todo - order by key
                .Paginate(pageNumber, pageSize);
    }

    public bool Exists(Guid id, bool? deleted)
    {
        IQueryable<Flag> flagsQuery =
            _flagDbContext
                .Flags
                .AsNoTracking();

        return
            deleted is null
                ? flagsQuery.IgnoreQueryFilters().Any(flag => flag.Id == id)
                : deleted.Value
                    ? flagsQuery.IgnoreQueryFilters().Any(flag => flag.Id == id && flag.DeletedAt.HasValue)
                    : flagsQuery.Any(flag => flag.Id == id);
    }

    public bool Exists(string name, Guid? parentId, bool? deleted)
    {
        IQueryable<Flag> flagsQuery =
            _flagDbContext
                .Flags
                .AsNoTracking();

        return
            deleted is null
                ? flagsQuery.IgnoreQueryFilters().Any(flag => flag.Name == name && flag.ParentId == parentId)
                : deleted.Value
                    ? flagsQuery.IgnoreQueryFilters().Any(flag => flag.Name == name && flag.ParentId == parentId && flag.DeletedAt.HasValue)
                    : flagsQuery.Any(flag => flag.Name == name && flag.ParentId == parentId);
    }

    public bool IsOn(Guid id)
    {
        return
            _flagDbContext
                .Flags
                .AsNoTracking()
                .Where(flag => flag.Id == id)
                .Select(flag => flag.IsOn)
                .FirstOrDefault();
    }

    public bool IsOn(string name, Guid? parentId)
    {
        return
            _flagDbContext
                .Flags
                .AsNoTracking()
                .Where(
                    flag =>
                        flag.Name == name
                        && flag.ParentId == parentId)
                .Select(flag => flag.IsOn)
                .FirstOrDefault();
    }
}
