using UltimateFlags.Abstraction.Entities;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.Storages;

internal interface IFlagQueryStorage
{
    public Flag? Read(Guid id, bool? deleted = false, bool includeParent = false);

    public Flag? Read(string key);

    public Flag? Read(string name, Guid? parentId);

    public IQueryable<Flag> ReadAll(IEnumerable<string> keys);

    public IQueryable<Flag> ReadAll(Guid? parentId, bool? deleted = false);

    public IQueryable<Flag> ReadAllDeleted(DateTime? fromInclusive = null, DateTime? toInclusive = null);

    public IPagedList<Flag> List(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize);

    public bool Exists(Guid id, bool? deleted = false);

    public bool Exists(string name, Guid? parentId, bool? deleted = false);

    public Task<Flag?> ReadAsync(
        Guid id,
        bool? deleted = false,
        bool includeParent = false,
        CancellationToken cancellationToken = default);

    public Task<Flag?> ReadAsync(string key, CancellationToken cancellationToken = default);

    public Task<Flag?> ReadAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default);

    public Task<IPagedList<Flag>> ListAsync(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    public Task<bool> ExistsAsync(Guid id, bool? deleted = false, CancellationToken cancellationToken = default);

    public Task<bool> ExistsAsync(string name, Guid? parentId, bool? deleted = false, CancellationToken cancellationToken = default);
}
