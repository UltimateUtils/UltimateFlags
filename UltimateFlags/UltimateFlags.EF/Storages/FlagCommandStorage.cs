using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.EF.Db;
using UltimateFlags.Helpers;

namespace UltimateFlags.EF.Storages;

public class FlagCommandStorage : IFlagCommandStorage
{
    private readonly ILogger<FlagCommandStorage> _logger;

    private readonly FlagDbContext _flagDbContext;

    private readonly IQueryable<Flag> _flags;

    public FlagCommandStorage(
        ILogger<FlagCommandStorage> logger,
        FlagDbContext flagDbContext)
    {
        _logger = logger;
        _flagDbContext = flagDbContext;
        _flags = flagDbContext.Flags;
    }

    public Flag? Get(Guid id, bool? deleted = false)
    {
        return
            deleted is null
                ? _flagDbContext.Flags.IgnoreQueryFilters().FirstOrDefault(f => f.Id == id)
                : deleted.Value
                    ? _flagDbContext
                        .Flags
                        .IgnoreQueryFilters()
                        .FirstOrDefault(
                            flag =>
                                flag.Id == id
                                && flag.DeletedAt.HasValue)
                    : _flagDbContext.Flags.Find(id);
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

    public IEnumerable<Flag> GetAll(Guid? parentId, bool? deleted)
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

    public bool Restore(Guid id)
    {
        int updatedCount =
            _flagDbContext
                .Flags
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(f => f.Id == id && f.DeletedAt.HasValue)
                .ExecuteUpdate(
                    setters =>
                        setters
                            .SetProperty(
                                f => f.DeletedAt,
                                (DateTime?)null));

        return updatedCount == 1;
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
}
