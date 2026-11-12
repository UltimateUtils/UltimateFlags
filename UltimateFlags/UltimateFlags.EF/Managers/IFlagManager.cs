using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.Managers;

internal interface IFlagManager
{
    public Flag Create(Flag entity);

    public Flag? Read(Guid id, bool? deleted = false);

    public Flag? Read(string name, Guid? parentId);

    public Flag? Read(string key);

    public IPagedList<Flag> GetAllDeleted(int pageNumber, int pageSize);

    public IPagedList<Flag> List(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize);

    public Flag Update(Guid id, FlagUpdateRequest updateRequest);

    public List<Flag> Delete(Guid id, Guid? concurrencyToken = null);

    public List<Flag> Purge(Guid id, Guid? concurrencyToken = null);

    public List<Flag> Restore(Guid id, Guid? concurrencyToken = null);

    public bool Exists(Guid id, bool? deleted = false);

    public bool Exists(string name, Guid? parentId, bool? deleted = false);

    public void Enable(Guid id);

    public void Enable(string key);

    public void Enable(string name, Guid? parentId);

    public void Disable(Guid id);

    public void Disable(string key);

    public void Disable(string name, Guid? parentId);

    public bool IsOn(string key);

    public int SaveChanges();

    public Task<Flag> CreateAsync(Flag entity, CancellationToken cancellationToken = default);

    public Task<Flag?> ReadAsync(Guid id, bool? deleted = false, CancellationToken cancellationToken = default);

    public Task<Flag?> ReadAsync(string name, Guid? parentId, CancellationToken cancellationToken = default);

    public Task<Flag?> ReadAsync(string key, CancellationToken cancellationToken = default);

    public Task<IPagedList<Flag>> GetAllDeletedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    public Task<IPagedList<Flag>> ListAsync(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    public Task<Flag> UpdateAsync(Guid id, FlagUpdateRequest updateRequest, CancellationToken cancellationToken = default);

    public Task<List<Flag>> DeleteAsync(Guid id, Guid? concurrencyToken = null, CancellationToken cancellationToken = default);

    public Task<List<Flag>> PurgeAsync(Guid id, Guid? concurrencyToken = null, CancellationToken cancellationToken = default);

    public Task<List<Flag>> RestoreAsync(Guid id, Guid? concurrencyToken = null, CancellationToken cancellationToken = default);

    public Task<bool> ExistsAsync(Guid id, bool? deleted = false, CancellationToken cancellationToken = default);

    public Task<bool> ExistsAsync(string name, Guid? parentId, bool? deleted = false, CancellationToken cancellationToken = default);

    public Task EnableAsync(Guid id, CancellationToken cancellationToken = default);

    public Task EnableAsync(string key, CancellationToken cancellationToken = default);

    public Task EnableAsync(string name, Guid? parentId, CancellationToken cancellationToken = default);

    public Task DisableAsync(Guid id, CancellationToken cancellationToken = default);

    public Task DisableAsync(string key, CancellationToken cancellationToken = default);

    public Task DisableAsync(string name, Guid? parentId, CancellationToken cancellationToken = default);

    public Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
