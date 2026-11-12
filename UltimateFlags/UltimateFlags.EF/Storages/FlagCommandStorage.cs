using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.EF.Db;
using UltimateFlags.Helpers;

namespace UltimateFlags.EF.Storages;

public class FlagCommandStorage : IFlagCommandStorage
{
    private readonly ILogger<FlagCommandStorage> _logger;

    private readonly FlagDbContext _flagDbContext;

    public FlagCommandStorage(
        ILogger<FlagCommandStorage> logger,
        FlagDbContext flagDbContext)
    {
        _logger = logger;
        _flagDbContext = flagDbContext;
    }

    public Flag? Get(Guid id, bool? deleted = false, bool includeParent = false)
    {
        IQueryable<Flag> flagsQuery =
            includeParent
                ? _flagDbContext.Flags.Include(f => f.Parent)
                : _flagDbContext.Flags;

        return
            deleted is null
                ? flagsQuery.IgnoreQueryFilters().FirstOrDefault(f => f.Id == id)
                : deleted.Value
                    ? flagsQuery
                        .IgnoreQueryFilters()
                        .FirstOrDefault(
                            flag =>
                                flag.Id == id
                                && flag.DeletedAt.HasValue)
                    : flagsQuery.FirstOrDefault(flag => flag.Id == id);
    }

    public Flag? Get(string name, Guid? parentId)
    {
        return
            _flagDbContext
                .Flags
                .FirstOrDefault(
                    flag =>
                        flag.Name == name
                        && flag.ParentId == parentId);
    }

    public IQueryable<Flag> GetAll(Guid? parentId, bool? deleted)
    {
        IQueryable<Flag> flagsQuery = _flagDbContext.Flags;

        return
            deleted is null
                ? flagsQuery
                    .IgnoreQueryFilters()
                    .Where(flag => flag.ParentId == parentId)
                : deleted.Value
                    ? flagsQuery
                        .IgnoreQueryFilters()
                        .Where(flag => flag.ParentId == parentId && flag.DeletedAt.HasValue)
                    : flagsQuery
                        .Where(flag => flag.ParentId == parentId);
    }

    public Flag Create(Flag flag)
    {
        EntityEntry<Flag> created = _flagDbContext.Flags.Add(flag);

        return created.Entity;
    }

    public Flag Update(Flag flag, Guid? concurrencyToken = null)
    {
        if (concurrencyToken.HasValue)
        {
            _flagDbContext.Entry(flag).Property(p => p.ConcurrencyToken).OriginalValue = concurrencyToken.Value;
        }

        EntityEntry<Flag> updated = _flagDbContext.Flags.Update(flag);

        return updated.Entity;
    }

    public Flag Delete(Flag flag, Guid? concurrencyToken = null)
    {
        if (concurrencyToken.HasValue)
        {
            _flagDbContext.Entry(flag).Property(p => p.ConcurrencyToken).OriginalValue = concurrencyToken.Value;
        }

        EntityEntry<Flag> softDeleted = _flagDbContext.Flags.Update(flag.Deleted());

        return softDeleted.Entity;
    }

    public Flag Purge(Flag flag, Guid? concurrencyToken = null)
    {
        if (concurrencyToken.HasValue)
        {
            _flagDbContext.Entry(flag).Property(p => p.ConcurrencyToken).OriginalValue = concurrencyToken.Value;
        }

        EntityEntry<Flag> hardDeleted = _flagDbContext.Flags.Remove(flag);

        return hardDeleted.Entity;
    }

    public Flag Restore(Flag flag, Guid? concurrencyToken = null)
    {
        if (concurrencyToken.HasValue)
        {
            _flagDbContext.Entry(flag).Property(p => p.ConcurrencyToken).OriginalValue = concurrencyToken.Value;
        }

        EntityEntry<Flag> restored = _flagDbContext.Flags.Update(flag.Restored());

        return restored.Entity;
    }

    public int Enable(Guid id)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Id == id)
                .ExecuteUpdate(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                true));
    }

    public int Enable(string key)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Key == key)
                .ExecuteUpdate(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                true));
    }

    public int Enable(string name, Guid? parentId)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Name == name && f.ParentId == parentId)
                .ExecuteUpdate(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                true));
    }

    public int Disable(Guid id)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Id == id)
                .ExecuteUpdate(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                false));
    }

    public int Disable(string key)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Key == key)
                .ExecuteUpdate(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                false));
    }

    public int Disable(string name, Guid? parentId)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Name == name && f.ParentId == parentId)
                .ExecuteUpdate(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                false));
    }

    public int SaveChanges()
    {
        return _flagDbContext.SaveChanges();
    }

    public Task<Flag?> GetAsync(
        Guid id,
        bool? deleted,
        bool includeParent = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Flag> flagsQuery =
            includeParent
                ? _flagDbContext.Flags.Include(f => f.Parent)
                : _flagDbContext.Flags;

        return
            deleted is null
                ? flagsQuery.IgnoreQueryFilters().FirstOrDefaultAsync(f => f.Id == id, cancellationToken)
                : deleted.Value
                    ? flagsQuery
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(
                            flag =>
                                flag.Id == id
                                && flag.DeletedAt.HasValue,
                            cancellationToken)
                    : flagsQuery.FirstOrDefaultAsync(flag => flag.Id == id, cancellationToken);
    }

    public Task<Flag?> GetAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        return
            _flagDbContext
                .Flags
                .FirstOrDefaultAsync(
                    flag =>
                        flag.Name == name
                        && flag.ParentId == parentId,
                    cancellationToken);
    }

    public async Task<Flag> CreateAsync(Flag flag, CancellationToken cancellationToken = default)
    {
        EntityEntry<Flag> created = await _flagDbContext.Flags.AddAsync(flag, cancellationToken);

        return created.Entity;
    }

    public Task<int> EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Id == id)
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                true),
                    cancellationToken);
    }

    public Task<int> EnableAsync(string key, CancellationToken cancellationToken = default)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Key == key)
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                true),
                    cancellationToken);
    }

    public Task<int> EnableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Name == name && f.ParentId == parentId)
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                true),
                    cancellationToken);
    }

    public Task<int> DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Id == id)
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                false),
                    cancellationToken);
    }

    public Task<int> DisableAsync(string key, CancellationToken cancellationToken = default)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Key == key)
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                false),
                    cancellationToken);
    }

    public Task<int> DisableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        return
            _flagDbContext
                .Flags
                .Where(f => f.Name == name && f.ParentId == parentId)
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.IsOn,
                                false),
                    cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _flagDbContext.SaveChangesAsync(cancellationToken);
    }
}
