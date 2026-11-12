using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Configuration.Storages;

internal interface IFlagCommandStorage
{
    public Flag? Get(Guid id, bool? deleted = false, bool includeParent = false);

    public Flag? Get(string name, Guid? parentId);

    public IEnumerable<Flag> GetAll(Guid? parentId, bool? deleted = false);

    public Flag Create(Flag flag);

    public Flag Update(Flag flag, Guid? concurrencyToken = null);

    public Flag Delete(Flag flag, Guid? concurrencyToken = null);

    public Flag Purge(Flag flag, Guid? concurrencyToken = null);

    public Flag Restore(Flag flag, Guid? concurrencyToken = null);

    public int Enable(Guid id);

    public int Enable(string key);

    public int Enable(string name, Guid? parentId);

    public int Disable(Guid id);

    public int Disable(string key);

    public int Disable(string name, Guid? parentId);

    public int SaveChanges();
}
