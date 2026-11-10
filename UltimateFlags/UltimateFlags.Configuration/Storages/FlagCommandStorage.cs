using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Storages;

namespace UltimateFlags.Configuration.Storages;

public class FlagCommandStorage : IFlagCommandStorage
{
    private readonly ILogger<FlagCommandStorage> _logger;

    public FlagCommandStorage(ILogger<FlagCommandStorage> logger)
    {
        _logger = logger;
    }

    public Flag? Get(Guid id, bool? deleted, bool includeParent = false)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Get)}(id, deleted, includeParent)", };
    }

    public Flag? Get(string name, Guid? parentId)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Get)}(name, parentId)", };
    }

    public IEnumerable<Flag> GetAll(Guid? parentId, bool? deleted)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(GetAll)}(parentId, deleted)", };
    }

    public Flag Create(Flag flag)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Create)}(flag)", };
    }

    public Flag Update(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Update)}(flag, concurrencyToken)", };
    }

    public Flag Delete(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Delete)}(flag, concurrencyToken)", };
    }

    public Flag Purge(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Purge)}(flag, concurrencyToken)", };
    }

    public Flag Restore(Flag flag, Guid? concurrencyToken = null)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Restore)}(flag, concurrencyToken)", };
    }

    public int Enable(Guid id)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Enable)}(id)", };
    }

    public int Enable(string key)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Enable)}(key)", };
    }

    public int Enable(string name, Guid? parentId)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Enable)}(name, parentId)", };
    }

    public int Disable(Guid id)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Disable)}(id)", };
    }

    public int Disable(string key)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Disable)}(key)", };
    }

    public int Disable(string name, Guid? parentId)
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(Disable)}(name, parentId)", };
    }

    public int SaveChanges()
    {
        throw new NotSupported { Area = $"{nameof(FlagCommandStorage)}.{nameof(SaveChanges)}()", };
    }
}
