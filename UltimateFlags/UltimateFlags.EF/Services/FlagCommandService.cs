using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Exceptions.ServerFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Converters;
using UltimateFlags.EF.Managers;

namespace UltimateFlags.EF.Services;

internal class FlagCommandService : IFlagCommandService
{
    private readonly ILogger<FlagCommandService> _logger;

    private readonly IFlagManager _flagManager;

    public FlagCommandService(
        ILogger<FlagCommandService> logger,
        IFlagManager flagManager)
    {
        _logger = logger;
        _flagManager = flagManager;
    }

    public FlagResponse Create(FlagCreationRequest creationRequest)
    {
        _validateFlagName();

        string parentKey = _getParentKey(creationRequest.ParentId);

        if (_flagManager.Exists(creationRequest.Name, creationRequest.ParentId, deleted: null))
        {
            throw new FlagDuplicateFound { Area = $"{nameof(FlagCommandService)}.{nameof(Create)}(creationRequest)", };
        }

        Flag createdEntity = _flagManager.Create(creationRequest.ToEntity(parentKey));

        return _flagManager.SaveChanges() > 0
            ? createdEntity.ToContract()
            : throw new FlagCreationFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Create)}(creationRequest)", };

        void _validateFlagName()
        {
            string name = creationRequest.Name;
            if (Regex.IsMatch(name, "[. ]"))
            {
                throw new InvalidFlagName("Flag name may not have the following characters: dot(.), space(' ').")
                {
                    Area = $"{nameof(FlagCommandService)}.{nameof(Create)}(creationRequest)",
                };
            }
        }

        string _getParentKey(Guid? parentId)
        {
            if (parentId is null)
            {
                return string.Empty;
            }

            // todo - improve - projection
            Flag? parent = _flagManager.Read(parentId.Value);
            if (parent == null)
            {
                throw new FlagParentNotFound
                {
                    Area = $"{nameof(FlagCommandService)}.{nameof(Create)}(creationRequest)",
                };
            }

            return parent.Key;
        }
    }

    public FlagResponse Update(Guid id, FlagUpdateRequest updateRequest)
    {
        Flag updatedEntity = _flagManager.Update(id, updateRequest);

        int saveChanges = _flagManager.SaveChanges();

        return
            saveChanges > 0
                ? updatedEntity.ToContract()
                : throw new FlagUpdateFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Update)}(id, updateRequest)", };
    }

    public IEnumerable<FlagResponse> Delete(Guid id, Guid? concurrencyToken = null)
    {
        IEnumerable<Flag> deletedEntities = _flagManager.Delete(id, concurrencyToken);

        return
            _flagManager.SaveChanges() > 0
                ? deletedEntities.ToContracts()
                : throw new FlagDeletionFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Delete)}(id, concurrencyToken)", };
    }

    public IEnumerable<FlagResponse> Purge(Guid id, Guid? concurrencyToken = null)
    {
        IEnumerable<Flag> purgedEntities = _flagManager.Purge(id, concurrencyToken);

        return
            _flagManager.SaveChanges() > 0
                ? purgedEntities.ToContracts()
                : throw new FlagPurgeFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Purge)}(id, concurrencyToken)", };
    }

    public IEnumerable<FlagResponse> Restore(Guid id, Guid? concurrencyToken = null)
    {
        IEnumerable<Flag> restoredEntities = _flagManager.Restore(id, concurrencyToken);

        return
            _flagManager.SaveChanges() > 0
                ? restoredEntities.ToContracts()
                : throw new FlagRestorationFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Restore)}(id)", };
    }

    public void Enable(Guid id)
    {
        _flagManager.Enable(id);
    }

    public void Enable(string key)
    {
        _flagManager.Enable(key);
    }

    public void Enable(string name, Guid? parentId)
    {
        _flagManager.Enable(name, parentId);
    }

    public void Disable(Guid id)
    {
        _flagManager.Disable(id);
    }

    public void Disable(string key)
    {
        _flagManager.Disable(key);
    }

    public void Disable(string name, Guid? parentId)
    {
        _flagManager.Disable(name, parentId);
    }

    public async Task<FlagResponse> CreateAsync(FlagCreationRequest creationRequest, CancellationToken cancellationToken = default)
    {
        _validateFlagName();

        string parentKey = await _getParentKeyAsync(creationRequest.ParentId);

        if (await _flagManager.ExistsAsync(creationRequest.Name, creationRequest.ParentId, deleted: null, cancellationToken))
        {
            throw new FlagDuplicateFound { Area = $"{nameof(FlagCommandService)}.{nameof(CreateAsync)}(creationRequest)", };
        }

        Flag createdEntity = await _flagManager.CreateAsync(creationRequest.ToEntity(parentKey), cancellationToken);

        return await _flagManager.SaveChangesAsync(cancellationToken) > 0
            ? createdEntity.ToContract()
            : throw new FlagCreationFailed { Area = $"{nameof(FlagCommandService)}.{nameof(CreateAsync)}(creationRequest)", };

        void _validateFlagName()
        {
            string name = creationRequest.Name;
            if (Regex.IsMatch(name, "[. ]"))
            {
                throw new InvalidFlagName("Flag name may not have the following characters: dot(.), space(' ').")
                {
                    Area = $"{nameof(FlagCommandService)}.{nameof(CreateAsync)}(creationRequest)",
                };
            }
        }

        async Task<string> _getParentKeyAsync(Guid? parentId)
        {
            if (parentId is null)
            {
                return string.Empty;
            }

            // todo - improve - projection
            Flag? parent = await _flagManager.ReadAsync(parentId.Value, cancellationToken: cancellationToken);
            if (parent == null)
            {
                throw new FlagParentNotFound
                {
                    Area = $"{nameof(FlagCommandService)}.{nameof(CreateAsync)}(creationRequest)",
                };
            }

            return parent.Key;
        }
    }

    public async Task<FlagResponse> UpdateAsync(
        Guid id,
        FlagUpdateRequest updateRequest,
        CancellationToken cancellationToken = default)
    {
        Flag updatedEntity = await _flagManager.UpdateAsync(id, updateRequest, cancellationToken);

        int saveChanges = await _flagManager.SaveChangesAsync(cancellationToken);

        return
            saveChanges > 0
                ? updatedEntity.ToContract()
                : throw new FlagUpdateFailed { Area = $"{nameof(FlagCommandService)}.{nameof(UpdateAsync)}(id, updateRequest)", };
    }

    public async Task<IEnumerable<FlagResponse>> DeleteAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Flag> deletedEntities = await _flagManager.DeleteAsync(id, concurrencyToken, cancellationToken);

        return
            await _flagManager.SaveChangesAsync(cancellationToken) > 0
                ? deletedEntities.ToContracts()
                : throw new FlagDeletionFailed { Area = $"{nameof(FlagCommandService)}.{nameof(DeleteAsync)}(id, concurrencyToken)", };
    }

    public async Task<IEnumerable<FlagResponse>> PurgeAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Flag> purgedEntities = await _flagManager.PurgeAsync(id, concurrencyToken, cancellationToken);

        return
            await _flagManager.SaveChangesAsync(cancellationToken) > 0
                ? purgedEntities.ToContracts()
                : throw new FlagPurgeFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Purge)}(id, concurrencyToken)", };
    }

    public async Task<IEnumerable<FlagResponse>> RestoreAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Flag> restoredEntities = await _flagManager.RestoreAsync(id, concurrencyToken, cancellationToken);

        return
            await _flagManager.SaveChangesAsync(cancellationToken) > 0
                ? restoredEntities.ToContracts()
                : throw new FlagRestorationFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Restore)}(id)", };
    }

    public Task EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _flagManager.EnableAsync(id, cancellationToken);
    }

    public Task EnableAsync(string key, CancellationToken cancellationToken = default)
    {
        return _flagManager.EnableAsync(key, cancellationToken);
    }

    public Task EnableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        return _flagManager.EnableAsync(name, parentId, cancellationToken);
    }

    public Task DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _flagManager.DisableAsync(id, cancellationToken);
    }

    public Task DisableAsync(string key, CancellationToken cancellationToken = default)
    {
        return _flagManager.DisableAsync(key, cancellationToken);
    }

    public Task DisableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        return _flagManager.DisableAsync(name, parentId, cancellationToken);
    }
}
