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

    public Task<Flag> CreateAsync(Flag entity)
    {
        throw new NotImplementedException();
    }

    public Task<Flag?> ReadAsync(Guid id, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public Task<Flag?> ReadAsync(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public Task<Flag?> ReadAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<Flag>> GetAllDeletedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<Flag>> ListAsync(string? searchString, bool? isOn, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Flag> UpdateAsync(Guid id, FlagUpdateRequest updateRequest)
    {
        throw new NotImplementedException();
    }

    public Task<List<Flag>> DeleteAsync(Guid id, Guid? concurrencyToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<List<Flag>> PurgeAsync(Guid id, Guid? concurrencyToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<List<Flag>> RestoreAsync(Guid id, Guid? concurrencyToken = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(string name, Guid? parentId, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public Task EnableAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task EnableAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task EnableAsync(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public Task DisableAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DisableAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task DisableAsync(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsOnAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
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
}
