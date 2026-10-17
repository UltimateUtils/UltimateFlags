using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Contracts;
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

    public Flag? Get(Guid id, bool? deleted)
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

    public Flag Update(Flag flag)
    {
        throw new NotImplementedException();
    }

    public int ExecuteUpdate(Guid id, FlagUpdateRequest contract)
    {
        throw new NotImplementedException();
    }

    public Flag Delete(Flag flag)
    {
        throw new NotImplementedException();
    }

    public int ExecuteDelete(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public Flag Purge(Flag flag)
    {
        throw new NotImplementedException();
    }

    public int ExecutePurge(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public bool Restore(Guid id)
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
