using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Config;

namespace UltimateFlags.Storages;

public class FlagCommandStorage : IFlagCommandStorage
{
    private readonly ILogger<FlagCommandStorage> _logger;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    private readonly Flag[] _flags;

    private readonly FlagsComplex _flagsComplex;

    public FlagCommandStorage(
        ILogger<FlagCommandStorage> logger,
        IOptionsSnapshot<UltimateFlagConfiguration> optionsSnapshot)
    {
        _logger = logger;
        _ultimateFlagConfiguration = optionsSnapshot.Value;
        _flags = optionsSnapshot.Value.Flags?.ToArray() ?? [];
        _flagsComplex = optionsSnapshot.Value.Flags.GetFlagsComplex();
    }

    public Flag? Get(Guid id, bool? deleted, bool includeParent = false)
    {
        throw new NotImplementedException();
    }

    public Flag? Get(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Flag> GetAll(Guid? parentId, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public Flag Create(Flag flag)
    {
        throw new NotImplementedException();
    }

    public Flag Update(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotImplementedException();
    }

    public Flag Delete(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotImplementedException();
    }

    public Flag Purge(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotImplementedException();
    }

    public Flag Restore(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotImplementedException();
    }

    public int Enable(Guid id)
    {
        throw new NotImplementedException();
    }

    public int Enable(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public int Disable(Guid id)
    {
        throw new NotImplementedException();
    }

    public int Disable(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public int SaveChanges()
    {
        throw new NotImplementedException();
    }
}
