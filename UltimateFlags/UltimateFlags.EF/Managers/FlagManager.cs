using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Exceptions.ServerFaults;
using UltimateFlags.Abstraction.Managers;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Helpers;
using UltimateFlags.Utils;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.Managers;

public class FlagManager : IFlagManager
{
    private readonly ILogger<FlagManager> _logger;

    private readonly IFlagQueryStorage _flagQueryStorage;

    private readonly IFlagCommandStorage _flagCommandStorage;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagManager(
        ILogger<FlagManager> logger,
        IFlagQueryStorage flagQueryStorage,
        IFlagCommandStorage flagCommandStorage,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _flagQueryStorage = flagQueryStorage;
        _flagCommandStorage = flagCommandStorage;
        _ultimateFlagConfiguration = options.Value;
    }

    public Flag Create(Flag entity)
    {
        return _flagCommandStorage.Create(entity);
    }

    public Flag? Read(Guid id, bool? deleted = false)
    {
        return _flagQueryStorage.Read(id, deleted);
    }

    public Flag? Read(string key)
    {
        string[] names = key.Split(Constants.KeyDelimiter);

        Guid? parentId = null;
        Flag? currentFlag = null;

        foreach (string name in names)
        {
            // todo - improve - projection - maybe with ReadId(name, parentId)
            currentFlag = _flagQueryStorage.Read(name, parentId);

            if (currentFlag is null)
                return null;

            parentId = currentFlag.Id;
        }

        return currentFlag;
    }

    public Flag? Read(string name, Guid? parentId)
    {
        return _flagQueryStorage.Read(name, parentId);
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

    public Flag Update(Guid id, FlagUpdateRequest contract)
    {
        Flag entity =
            _flagCommandStorage.Get(id)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Update)}(id, contract)",
            };

        return _flagCommandStorage.Update(entity.UpdatedFrom(contract));
    }

    public int ExecuteUpdate(Guid id, FlagUpdateRequest contract)
    {
        if (!_flagQueryStorage.Exists(id))
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(ExecuteUpdate)}(id, contract)",
            };
        }

        int updatedCount = _flagCommandStorage.ExecuteUpdate(id, contract);
        if (updatedCount == 1)
            return updatedCount;

        throw new FlagUpdateFailed
        {
            Area = $"{nameof(FlagManager)}.{nameof(ExecuteUpdate)}(id, contract)",
        };
    }

    public List<Flag> Delete(Guid id)
    {
        // need to enumerate here
        return [.. _Delete(id, purge: false)];
    }

    public int ExecuteDelete(Guid id)
    {
        IEnumerable<Guid> idsToDelete = _GetAllDescendantIds(id, deleted: false);
        return _flagCommandStorage.ExecuteDelete(idsToDelete);
    }

    public List<Flag> Purge(Guid id)
    {
        // need to enumerate here
        return [.. _Delete(id, purge: true)];
    }

    public int ExecutePurge(Guid id)
    {
        IEnumerable<Guid> idsToPurge = _GetAllDescendantIds(id, deleted: true);
        return _flagCommandStorage.ExecutePurge(idsToPurge);
    }

    public int ExecutePurge(DateTime? fromInclusive = null, DateTime? toInclusive = null)
    {
        IEnumerable<Guid> rootIds =
            _flagQueryStorage
                .ReadAllDeleted(fromInclusive, toInclusive)
                .Select(flag => flag.Id);

        IEnumerable<Guid> idsToPurge = _GetAllDescendantIds(rootIds, deleted: true);
        return _flagCommandStorage.ExecutePurge(idsToPurge);
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

    public bool IsOn(Guid id)
    {
        // todo - improve - projection

        Flag entity =
            _flagQueryStorage.Read(id)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(IsOn)}(id)",
            };

        return entity.IsOn;
    }

    public virtual bool IsOn(string key)
    {
        string[] names = key.Split(Constants.KeyDelimiter);

        Guid? parentId = null;
        Flag? currentFlag = null;

        foreach (string name in names)
        {
            // todo - improve - projection
            currentFlag = _flagQueryStorage.Read(name, parentId);
            if (currentFlag is null || !currentFlag.IsOn)
                return false;

            parentId = currentFlag.Id;
        }

        return currentFlag?.IsOn ?? false;
    }

    public int SaveChanges()
    {
        return _flagCommandStorage.SaveChanges();
    }

    private IEnumerable<Flag> _Delete(Guid id, bool purge)
    {
        // todo - improve
        Flag rootToDelete =
            _flagQueryStorage.Read(id, deleted: purge)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Delete)}(id)",
            };

        Queue<Flag> flagsToDelete = [];

        flagsToDelete.Enqueue(rootToDelete);

        while (flagsToDelete.Count > 0)
        {
            Flag curr = flagsToDelete.Dequeue();
            Flag deleted =
                purge
                    ? _flagCommandStorage.Purge(curr)
                    : _flagCommandStorage.Delete(curr);

            yield return deleted;

            IEnumerable<Flag> nextGeneration = _flagQueryStorage.ReadAll(parentId: deleted.Id, deleted: purge);

            // todo - improve with PushRange()
            foreach (Flag nextFlag in nextGeneration)
            {
                flagsToDelete.Enqueue(nextFlag);
            }
        }
    }

    private IEnumerable<Guid> _GetAllDescendantIds(IEnumerable<Guid> rootIds, bool? deleted)
    {
        // todo - test algorithm

        HashSet<Guid> visited = [];

        foreach (Guid rootId in rootIds)
        {
            if (!visited.Add(rootId))
                continue;

            IEnumerable<Guid> descendentIds = _GetAllDescendantIds(rootId, deleted);

            foreach (Guid descendentId in descendentIds)
            {
                if (visited.Add(descendentId))
                    yield return descendentId;
            }
        }
    }

    private IEnumerable<Guid> _GetAllDescendantIds(Guid rootId, bool? deleted)
    {
        if (!_flagQueryStorage.Exists(rootId, deleted))
        {
            throw new FlagNotFound
            {
                Area = $"{nameof(FlagManager)}.{nameof(Delete)}(id)",
            };
        }

        yield return rootId;

        List<Guid> frontier = [rootId];
        while (frontier.Count > 0)
        {
            List<Guid> nextBag = [];

            foreach (Guid parentId in frontier)
            {
                Guid[] nextGenerationIds = [.. _flagQueryStorage.ReadAll(parentId, deleted).Select(f => f.Id)];
                nextBag.AddRange(nextGenerationIds);

                foreach (Guid nextGenerationId in nextGenerationIds)
                {
                    yield return nextGenerationId;
                }
            }

            frontier = nextBag;
        }
    }
}
