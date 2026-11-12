using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.EF.Storages;

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

    public Task<Flag?> GetAsync(Guid id, bool? deleted = false, bool includeParent = false);

    public Task<Flag?> GetAsync(string name, Guid? parentId);

    public Task<IEnumerable<Flag>> GetAllAsync(Guid? parentId, bool? deleted = false);

    public Task<Flag> CreateAsync(Flag flag);

    public Task<Flag> UpdateAsync(Flag flag, Guid? concurrencyToken = null);

    public Task<Flag> DeleteAsync(Flag flag, Guid? concurrencyToken = null);

    public Task<Flag> PurgeAsync(Flag flag, Guid? concurrencyToken = null);

    public Task<Flag> RestoreAsync(Flag flag, Guid? concurrencyToken = null);

    public Task<int> EnableAsync(Guid id);

    public Task<int> EnableAsync(string key);

    public Task<int> EnableAsync(string name, Guid? parentId);

    public Task<int> DisableAsync(Guid id);

    public Task<int> DisableAsync(string key);

    public Task<int> DisableAsync(string name, Guid? parentId);

    public Task<int> SaveChangesAsync();
}
