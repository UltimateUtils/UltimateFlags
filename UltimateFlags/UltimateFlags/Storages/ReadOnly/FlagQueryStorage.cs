using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Config;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Storages.ReadOnly;

public class FlagQueryStorage : IFlagQueryStorage
{
    private readonly ILogger<FlagQueryStorage> _logger;

    private readonly FlagsComplex _flagsComplex;

    public FlagQueryStorage(
        ILogger<FlagQueryStorage> logger,
        FlagsComplex flagsComplex)
    {
        _logger = logger;
        _flagsComplex = flagsComplex;
    }

    public Flag? Read(Guid id, bool? deleted = false)
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

    public IQueryable<Flag> ReadAll(IEnumerable<string> keys)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Flag> ReadAll(Guid? parentId, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Flag> ReadAllDeleted(DateTime? fromInclusive = null, DateTime? toInclusive = null)
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
