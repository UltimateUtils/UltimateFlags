using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.EF.Storages;

internal interface IFlagCommandStorage
{
    public Flag? Get(Guid id, bool? deleted = false, bool includeParent = false);

    public Flag? Get(string name, Guid? parentId);

    public IQueryable<Flag> GetAll(Guid? parentId, bool? deleted = false);

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

    public Task<Flag?> GetAsync(Guid id, bool? deleted = false, bool includeParent = false, CancellationToken cancellationToken = default);

    public Task<Flag?> GetAsync(string name, Guid? parentId, CancellationToken cancellationToken = default);

    public Task<Flag> CreateAsync(Flag flag, CancellationToken cancellationToken = default);

    public Task<int> EnableAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<int> EnableAsync(string key, CancellationToken cancellationToken = default);

    public Task<int> EnableAsync(string name, Guid? parentId, CancellationToken cancellationToken = default);

    public Task<int> DisableAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<int> DisableAsync(string key, CancellationToken cancellationToken = default);

    public Task<int> DisableAsync(string name, Guid? parentId, CancellationToken cancellationToken = default);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
