using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Config;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Storages;

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

    public IQueryable<Flag> ReadAll(IEnumerable<string> keys)
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
