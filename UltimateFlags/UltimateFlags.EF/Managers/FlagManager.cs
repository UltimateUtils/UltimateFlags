using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Exceptions.ServerFaults;
using UltimateFlags.EF.Storages;
using UltimateFlags.Helpers;
using UltimatePagination;
using UltimatePagination.Abstraction;
using UltimatePagination.EF;

namespace UltimateFlags.EF.Managers;

internal class FlagManager : IFlagManager
{
    private readonly ILogger<FlagManager> _logger;

    private readonly IFlagQueryStorage _flagQueryStorage;

    private readonly IFlagCommandStorage _flagCommandStorage;

    public FlagManager(
        ILogger<FlagManager> logger,
        IFlagQueryStorage flagQueryStorage,
        IFlagCommandStorage flagCommandStorage)
    {
        _logger = logger;
        _flagQueryStorage = flagQueryStorage;
        _flagCommandStorage = flagCommandStorage;
    }

    public Flag Create(Flag entity)
    {
        return _flagCommandStorage.Create(entity);
    }

    public Flag? Read(Guid id, bool? deleted = false)
    {
        return _flagQueryStorage.Read(id, deleted);
    }

    public Flag? Read(string name, Guid? parentId)
    {
        return _flagQueryStorage.Read(name, parentId);
    }

    public Flag? Read(string key)
    {
        return _flagQueryStorage.Read(key);
    }

    public IPagedList<Flag> GetAllDeleted(int pageNumber, int pageSize)
    {
        return
            _flagQueryStorage
                .ReadAllDeleted()
                .Paginate(pageNumber, pageSize);
    }

    public IPagedList<Flag> List(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize)
    {
        return
            _flagQueryStorage
                .List(
                    searchString,
                    isOn,
                    pageNumber,
                    pageSize);
    }

    public Flag Update(Guid id, FlagUpdateRequest updateRequest)
    {
        Flag entity =
            _flagCommandStorage.Get(id)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Update)}(id, contract)",
            };

        return _flagCommandStorage.Update(entity.UpdatedFrom(updateRequest), updateRequest.ConcurrencyToken);
    }

    public List<Flag> Delete(Guid id, Guid? concurrencyToken = null)
    {
        // need to enumerate here
        return [.. _Delete(id, concurrencyToken)];
    }

    public List<Flag> Purge(Guid id, Guid? concurrencyToken = null)
    {
        // need to enumerate here
        return [.. _Purge(id, concurrencyToken)];
    }

    public List<Flag> Restore(Guid id, Guid? concurrencyToken = null)
    {
        // need to enumerate here
        return [.. _Restore(id, concurrencyToken)];
    }

    public bool Exists(Guid id, bool? deleted = false)
    {
        return _flagQueryStorage.Exists(id, deleted);
    }

    public bool Exists(
        string name,
        Guid? parentId,
        bool? deleted = false)
    {
        return _flagQueryStorage.Exists(name, parentId, deleted);
    }

    public void Enable(Guid id)
    {
        int enabledCount = _flagCommandStorage.Enable(id);

        if (enabledCount == 1)
            return;

        Flag? flag = _flagQueryStorage.Read(id);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Enable)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(Enable)}(id)",
        };
    }

    public void Enable(string key)
    {
        int enabledCount = _flagCommandStorage.Enable(key);

        if (enabledCount == 1)
            return;

        Flag? flag = _flagQueryStorage.Read(key);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Enable)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(Enable)}(id)",
        };
    }

    public void Enable(string name, Guid? parentId)
    {
        int enabledCount = _flagCommandStorage.Enable(name, parentId);

        if (enabledCount == 1)
            return;

        Flag? flag = _flagQueryStorage.Read(name, parentId);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Enable)}(name, parentId)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(Enable)}(name, parentId)",
        };
    }

    public void Disable(Guid id)
    {
        int disabledCount = _flagCommandStorage.Disable(id);

        if (disabledCount == 1)
            return;

        Flag? flag = _flagQueryStorage.Read(id);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Disable)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(Disable)}(id)",
        };
    }

    public void Disable(string key)
    {
        int disabledCount = _flagCommandStorage.Disable(key);

        if (disabledCount == 1)
            return;

        Flag? flag = _flagQueryStorage.Read(key);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Disable)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(Disable)}(id)",
        };
    }

    public void Disable(string name, Guid? parentId)
    {
        int disabledCount = _flagCommandStorage.Disable(name, parentId);

        if (disabledCount == 1)
            return;

        Flag? flag = _flagQueryStorage.Read(name, parentId);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Disable)}(name, parentId)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(Disable)}(name, parentId)",
        };
    }

    public bool IsOn(string key)
    {
        string[] ancestorKeys = [.. key.GetAncestorKeys(inclusive: true)];

        bool[] isOns =
        [
            .. _flagQueryStorage
                .ReadAll(ancestorKeys)
                /*.Where(x => x.IsOn)*/ // todo - performance check
                .Select(x => x.IsOn)
        ];

        return isOns.Length == ancestorKeys.Length
            && isOns.All(isOn => isOn);
    }

    public int SaveChanges()
    {
        try
        {
            return _flagCommandStorage.SaveChanges();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogWarning(e, "{ErrorMessage}", e.Message);

            throw new ConcurrencyConflict
            {
                Area = $"{nameof(FlagManager)}.{nameof(SaveChanges)}()",
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{ErrorMessage}", e.Message);
            return 0;
        }
    }

    public async Task<Flag> CreateAsync(Flag entity, CancellationToken cancellationToken = default)
    {
        return await _flagCommandStorage.CreateAsync(entity, cancellationToken);
    }

    public async Task<Flag?> ReadAsync(Guid id, bool? deleted, CancellationToken cancellationToken = default)
    {
        return await _flagQueryStorage.ReadAsync(id, deleted, cancellationToken: cancellationToken);
    }

    public async Task<Flag?> ReadAsync(string name, Guid? parentId, CancellationToken cancellationToken = default)
    {
        return await _flagQueryStorage.ReadAsync(name, parentId, cancellationToken);
    }

    public async Task<Flag?> ReadAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _flagQueryStorage.ReadAsync(key, cancellationToken);
    }

    public async Task<IPagedList<Flag>> GetAllDeletedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return
            await
                _flagQueryStorage
                    .ReadAllDeleted()
                    .PaginateAsync(pageNumber, pageSize, cancellationToken);
    }

    public Task<IPagedList<Flag>> ListAsync(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return
            _flagQueryStorage
                .ListAsync(
                    searchString,
                    isOn,
                    pageNumber,
                    pageSize,
                    cancellationToken);
    }

    public async Task<Flag> UpdateAsync(
        Guid id,
        FlagUpdateRequest updateRequest,
        CancellationToken cancellationToken = default)
    {
        Flag entity =
            await _flagCommandStorage.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(UpdateAsync)}(id, contract)",
            };

        return _flagCommandStorage.Update(entity.UpdatedFrom(updateRequest), updateRequest.ConcurrencyToken);
    }

    public async Task<List<Flag>> DeleteAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        // todo - async enumerable
        IEnumerable<Flag> deleted = await _DeleteAsync(id, concurrencyToken, cancellationToken);

        // need to enumerate here
        return [.. deleted];
    }

    public async Task<List<Flag>> PurgeAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        // todo - async enumerable
        IEnumerable<Flag> purged = await _PurgeAsync(id, concurrencyToken, cancellationToken);

        // need to enumerate here
        return [.. purged];
    }

    public async Task<List<Flag>> RestoreAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        // todo - async enumerable
        IEnumerable<Flag> restored = await _RestoreAsync(id, concurrencyToken, cancellationToken);

        // need to enumerate here
        return [.. restored];
    }

    public async Task<bool> ExistsAsync(
        Guid id,
        bool? deleted,
        CancellationToken cancellationToken = default)
    {
        return await _flagQueryStorage.ExistsAsync(id, deleted, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string name,
        Guid? parentId,
        bool? deleted,
        CancellationToken cancellationToken = default)
    {
        return await _flagQueryStorage.ExistsAsync(name, parentId, deleted, cancellationToken);
    }

    public async Task EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        int enabledCount = await _flagCommandStorage.EnableAsync(id, cancellationToken);

        if (enabledCount == 1)
            return;

        Flag? flag = await _flagQueryStorage.ReadAsync(id, cancellationToken: cancellationToken);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(EnableAsync)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(EnableAsync)}(id)",
        };
    }

    public async Task EnableAsync(string key, CancellationToken cancellationToken = default)
    {
        int enabledCount = await _flagCommandStorage.EnableAsync(key, cancellationToken);

        if (enabledCount == 1)
            return;

        Flag? flag = await _flagQueryStorage.ReadAsync(key, cancellationToken);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(EnableAsync)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(EnableAsync)}(id)",
        };
    }

    public async Task EnableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        int enabledCount = await _flagCommandStorage.EnableAsync(name, parentId, cancellationToken);

        if (enabledCount == 1)
            return;

        Flag? flag = await _flagQueryStorage.ReadAsync(name, parentId, cancellationToken);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(EnableAsync)}(name, parentId)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(EnableAsync)}(name, parentId)",
        };
    }

    public async Task DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        int disabledCount = await _flagCommandStorage.DisableAsync(id, cancellationToken);

        if (disabledCount == 1)
            return;

        Flag? flag = await _flagQueryStorage.ReadAsync(id, cancellationToken: cancellationToken);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(DisableAsync)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(DisableAsync)}(id)",
        };
    }

    public async Task DisableAsync(string key, CancellationToken cancellationToken = default)
    {
        int disabledCount = await _flagCommandStorage.DisableAsync(key, cancellationToken);

        if (disabledCount == 1)
            return;

        Flag? flag = await _flagQueryStorage.ReadAsync(key, cancellationToken);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(DisableAsync)}(id)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(DisableAsync)}(id)",
        };
    }

    public async Task DisableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        int disabledCount = await _flagCommandStorage.DisableAsync(name, parentId, cancellationToken);

        if (disabledCount == 1)
            return;

        Flag? flag = await _flagQueryStorage.ReadAsync(name, parentId, cancellationToken);

        if (flag is null)
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(DisableAsync)}(name, parentId)",
            };
        }

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(DisableAsync)}(name, parentId)",
        };
    }

    public async Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default)
    {
        string[] ancestorKeys = [.. key.GetAncestorKeys(inclusive: true)];

        bool[] isOns =
            await _flagQueryStorage
                .ReadAll(ancestorKeys)
                /*.Where(x => x.IsOn)*/ // todo - performance check
                .Select(x => x.IsOn)
                .ToArrayAsync(cancellationToken);

        return isOns.Length == ancestorKeys.Length
            && isOns.All(isOn => isOn);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _flagCommandStorage.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogWarning(e, "{ErrorMessage}", e.Message);

            throw new ConcurrencyConflict
            {
                Area = $"{nameof(FlagManager)}.{nameof(SaveChangesAsync)}()",
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{ErrorMessage}", e.Message);
            return 0;
        }
    }

    private IEnumerable<Flag> _Delete(Guid id, Guid? concurrencyToken)
    {
        Flag rootToDelete =
            _flagCommandStorage.Get(id, deleted: false)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(_Delete)}(id)",
            };

        Flag deletedRoot = _flagCommandStorage.Delete(rootToDelete, concurrencyToken);

        yield return deletedRoot;

        IEnumerable<Flag> secondGeneration = _flagCommandStorage.GetAll(parentId: deletedRoot.Id, deleted: false);
        Queue<Flag> flagsToDelete = new(secondGeneration);

        while (flagsToDelete.Count > 0)
        {
            Flag curr = flagsToDelete.Dequeue();
            Flag deleted = _flagCommandStorage.Delete(curr);

            yield return deleted;

            IEnumerable<Flag> nextGeneration = _flagCommandStorage.GetAll(parentId: deleted.Id, deleted: false);

            // todo - improve with PushRange()
            foreach (Flag nextFlag in nextGeneration)
            {
                flagsToDelete.Enqueue(nextFlag);
            }
        }
    }

    private async Task<IEnumerable<Flag>> _DeleteAsync(Guid id, Guid? concurrencyToken, CancellationToken cancellationToken = default)
    {
        Flag rootToDelete =
            await _flagCommandStorage.GetAsync(id, deleted: false, cancellationToken: cancellationToken)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(_DeleteAsync)}(id)",
            };

        Flag deletedRoot = _flagCommandStorage.Delete(rootToDelete, concurrencyToken);

        List<Flag> ret = [deletedRoot];

        Flag[] secondGeneration = await _flagCommandStorage.GetAll(parentId: deletedRoot.Id, deleted: false).ToArrayAsync(cancellationToken);
        Queue<Flag> flagsToDelete = new(secondGeneration);

        while (flagsToDelete.Count > 0)
        {
            Flag curr = flagsToDelete.Dequeue();
            Flag deleted = _flagCommandStorage.Delete(curr);

            ret.Add(deleted);

            Flag[] nextGeneration = await _flagCommandStorage.GetAll(parentId: deleted.Id, deleted: false).ToArrayAsync(cancellationToken);

            // todo - improve with PushRange()
            foreach (Flag nextFlag in nextGeneration)
            {
                flagsToDelete.Enqueue(nextFlag);
            }
        }

        return ret;
    }

    private IEnumerable<Flag> _Purge(Guid id, Guid? concurrencyToken)
    {
        Flag rootToDelete =
            _flagQueryStorage.Read(id, deleted: true)
            ?? throw new FlagNotDeleted
            {
                Area = $"{nameof(FlagManager)}.{nameof(Delete)}(id)",
            };

        Flag purgedRoot = _flagCommandStorage.Purge(rootToDelete, concurrencyToken);

        yield return purgedRoot;

        IEnumerable<Flag> secondGeneration = _flagCommandStorage.GetAll(parentId: purgedRoot.Id, deleted: true);
        Queue<Flag> flagsToPurge = new(secondGeneration);

        while (flagsToPurge.Count > 0)
        {
            Flag curr = flagsToPurge.Dequeue();
            Flag purged = _flagCommandStorage.Purge(curr);

            yield return purged;

            IEnumerable<Flag> nextGeneration = _flagCommandStorage.GetAll(parentId: purged.Id, deleted: true);

            // todo - improve with PushRange()
            foreach (Flag nextFlag in nextGeneration)
            {
                flagsToPurge.Enqueue(nextFlag);
            }
        }
    }

    private async Task<IEnumerable<Flag>> _PurgeAsync(Guid id, Guid? concurrencyToken, CancellationToken cancellationToken = default)
    {
        Flag rootToDelete =
            await _flagQueryStorage.ReadAsync(id, deleted: true, cancellationToken: cancellationToken)
            ?? throw new FlagNotDeleted
            {
                Area = $"{nameof(FlagManager)}.{nameof(Delete)}(id)",
            };

        Flag purgedRoot = _flagCommandStorage.Purge(rootToDelete, concurrencyToken);

        List<Flag> ret = [purgedRoot];

        Flag[] secondGeneration = await _flagCommandStorage.GetAll(parentId: purgedRoot.Id, deleted: true).ToArrayAsync(cancellationToken);
        Queue<Flag> flagsToPurge = new(secondGeneration);

        while (flagsToPurge.Count > 0)
        {
            Flag curr = flagsToPurge.Dequeue();
            Flag purged = _flagCommandStorage.Purge(curr);

            ret.Add(purged);

            Flag[] nextGeneration = await _flagCommandStorage.GetAll(parentId: purged.Id, deleted: true).ToArrayAsync(cancellationToken);

            // todo - improve with PushRange()
            foreach (Flag nextFlag in nextGeneration)
            {
                flagsToPurge.Enqueue(nextFlag);
            }
        }

        return ret;
    }

    private IEnumerable<Flag> _Restore(Guid id, Guid? concurrencyToken = null)
    {
        Flag leafToRestore =
            _flagCommandStorage.Get(id, deleted: null)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(_Delete)}(id)",
            };

        if (!leafToRestore.DeletedAt.HasValue)
        {
            throw new FlagNotDeleted
            {
                Area = $"{nameof(FlagManager)}.{nameof(_Restore)}(id, concurrencyToken)",
            };
        }

        Flag restoreResponse = _flagCommandStorage.Restore(leafToRestore, concurrencyToken);

        yield return restoreResponse;

        Flag restoredLeaf =
            _flagCommandStorage.Get(restoreResponse.Id, deleted: null, includeParent: true)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(_Restore)}(id, concurrencyToken)",
            };

        foreach (Flag ancestor in _restoreAncestors(restoredLeaf))
        {
            yield return ancestor;
        }

        yield break;

        IEnumerable<Flag> _restoreAncestors(Flag curr)
        {
            while (curr.Parent is { DeletedAt: not null })
            {
                Flag restored = _flagCommandStorage.Restore(curr.Parent);
                yield return restored;

                curr =
                    _flagCommandStorage.Get(restored.Id, deleted: null, includeParent: true)
                    ?? throw new FlagNotFound
                    {
                        Area = $"{nameof(FlagManager)}.{nameof(_Restore)}(id, concurrencyToken)",
                    };
            }
        }
    }

    private async Task<IEnumerable<Flag>> _RestoreAsync(Guid id, Guid? concurrencyToken = null, CancellationToken cancellationToken = default)
    {
        Flag leafToRestore =
            await _flagCommandStorage.GetAsync(id, deleted: null, cancellationToken: cancellationToken)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(_Delete)}(id)",
            };

        if (!leafToRestore.DeletedAt.HasValue)
        {
            throw new FlagNotDeleted
            {
                Area = $"{nameof(FlagManager)}.{nameof(_Restore)}(id, concurrencyToken)",
            };
        }

        Flag restoreResponse = _flagCommandStorage.Restore(leafToRestore, concurrencyToken);

        List<Flag> ret = [restoreResponse];

        Flag restoredLeaf =
            await _flagCommandStorage.GetAsync(restoreResponse.Id, deleted: null, includeParent: true, cancellationToken)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(_Restore)}(id, concurrencyToken)",
            };

        foreach (Flag ancestor in _restoreAncestors(restoredLeaf))
        {
            ret.Add(ancestor);
        }

        return ret;

        // todo - async enumerable
        IEnumerable<Flag> _restoreAncestors(Flag curr)
        {
            while (curr.Parent is { DeletedAt: not null })
            {
                Flag restored = _flagCommandStorage.Restore(curr.Parent);
                yield return restored;

                curr =
                    _flagCommandStorage.Get(restored.Id, deleted: null, includeParent: true)
                    ?? throw new FlagNotFound
                    {
                        Area = $"{nameof(FlagManager)}.{nameof(_Restore)}(id, concurrencyToken)",
                    };
            }
        }
    }
}
