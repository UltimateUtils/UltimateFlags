using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Config;

namespace UltimateFlags.Storages.ReadOnly;

public class FlagCommandStorage : IFlagCommandStorage
{
    private readonly ILogger<FlagCommandStorage> _logger;

    private readonly FlagsComplex _flagsComplex;

    public FlagCommandStorage(
        ILogger<FlagCommandStorage> logger,
        FlagsComplex flagsComplex)
    {
        _logger = logger;
        _flagsComplex = flagsComplex;
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
