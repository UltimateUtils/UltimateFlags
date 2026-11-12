using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Configuration.Storages;
using UltimateFlags.Helpers;
using UltimatePagination;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Configuration.Managers;

internal class FlagManager : IFlagManager
{
    private readonly ILogger<FlagManager> _logger;

    private readonly IFlagQueryStorage _flagQueryStorage;

    public FlagManager(
        ILogger<FlagManager> logger,
        IFlagQueryStorage flagQueryStorage)
    {
        _logger = logger;
        _flagQueryStorage = flagQueryStorage;
    }

    public Flag? Read(Guid id, bool? deleted = false)
    {
        return _flagQueryStorage.Read(id, deleted);
    }

    public Flag? Read(string name, Guid? parentId)
    {
        return _flagQueryStorage.Read(name, parentId);
    }

    public Flag? Read(string key)
    {
        return _flagQueryStorage.Read(key);
    }

    public IPagedList<Flag> GetAllDeleted(int pageNumber, int pageSize)
    {
        return
            _flagQueryStorage
                .ReadAllDeleted()
                .Paginate(pageNumber, pageSize);
    }

    public IPagedList<Flag> List(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize)
    {
        return
            _flagQueryStorage
                .List(
                    searchString,
                    isOn,
                    pageNumber,
                    pageSize);
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
