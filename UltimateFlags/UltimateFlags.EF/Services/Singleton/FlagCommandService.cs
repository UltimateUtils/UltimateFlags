using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Exceptions.ServerFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Converters;
using UltimateFlags.EF.Managers;

namespace UltimateFlags.EF.Services.Singleton;

internal class FlagCommandService : IFlagCommandService
{
    private readonly ILogger<FlagCommandService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FlagCommandService(
        ILogger<FlagCommandService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public FlagResponse Create(FlagCreationRequest creationRequest)
    {
        _validateFlagName();

        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        string parentKey = _getParentKey(flagManager, creationRequest.ParentId);

        if (flagManager.Exists(creationRequest.Name, creationRequest.ParentId, deleted: null))
        {
            throw new FlagDuplicateFound { Area = $"{nameof(FlagCommandService)}.{nameof(Create)}(creationRequest)", };
        }

        Flag createdEntity = flagManager.Create(creationRequest.ToEntity(parentKey));

        return flagManager.SaveChanges() > 0
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

        static string _getParentKey(IFlagManager flagManager, Guid? parentId)
        {
            if (parentId is null)
            {
                return string.Empty;
            }

            // todo - improve - projection
            Flag? parent = flagManager.Read(parentId.Value);
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
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag updatedEntity = flagManager.Update(id, updateRequest);

        return
            flagManager.SaveChanges() > 0
                ? updatedEntity.ToContract()
                : throw new FlagUpdateFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Update)}(id, updateRequest)", };
    }

    public IEnumerable<FlagResponse> Delete(Guid id, Guid? concurrencyToken = null)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> deletedEntities = flagManager.Delete(id);

        return
            flagManager.SaveChanges() > 0
                ? deletedEntities.ToContracts()
                : throw new FlagDeletionFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Delete)}(id, concurrencyToken)", };
    }

    public IEnumerable<FlagResponse> Purge(Guid id, Guid? concurrencyToken = null)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> purgedEntity = flagManager.Purge(id, concurrencyToken);

        return
            flagManager.SaveChanges() > 0
                ? purgedEntity.ToContracts()
                : throw new FlagPurgeFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Purge)}(id, concurrencyToken)", };
    }

    public IEnumerable<FlagResponse> Restore(Guid id, Guid? concurrencyToken = null)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> restoredEntities = flagManager.Restore(id, concurrencyToken);

        return
            flagManager.SaveChanges() > 0
                ? restoredEntities.ToContracts()
                : throw new FlagRestorationFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Restore)}(id)", };
    }

    public void Enable(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Enable(id);
    }

    public void Enable(string key)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Enable(key);
    }

    public void Enable(string name, Guid? parentId)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Enable(name, parentId);
    }

    public void Disable(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Disable(id);
    }

    public void Disable(string key)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Disable(key);
    }

    public void Disable(string name, Guid? parentId)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Disable(name, parentId);
    }

    public async Task<FlagResponse> CreateAsync(FlagCreationRequest creationRequest, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        _validateFlagName();

        string parentKey = await _getParentKeyAsync(creationRequest.ParentId);

        if (await flagManager.ExistsAsync(creationRequest.Name, creationRequest.ParentId, deleted: null, cancellationToken))
        {
            throw new FlagDuplicateFound { Area = $"{nameof(FlagCommandService)}.{nameof(CreateAsync)}(creationRequest)", };
        }

        Flag createdEntity = await flagManager.CreateAsync(creationRequest.ToEntity(parentKey), cancellationToken);

        return await flagManager.SaveChangesAsync(cancellationToken) > 0
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
            Flag? parent = await flagManager.ReadAsync(parentId.Value, cancellationToken: cancellationToken);
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
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag updatedEntity = await flagManager.UpdateAsync(id, updateRequest, cancellationToken);

        int saveChanges = await flagManager.SaveChangesAsync(cancellationToken);

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
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> deletedEntities = await flagManager.DeleteAsync(id, concurrencyToken, cancellationToken);

        return
            await flagManager.SaveChangesAsync(cancellationToken) > 0
                ? deletedEntities.ToContracts()
                : throw new FlagDeletionFailed { Area = $"{nameof(FlagCommandService)}.{nameof(DeleteAsync)}(id, concurrencyToken)", };
    }

    public async Task<IEnumerable<FlagResponse>> PurgeAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> purgedEntities = await flagManager.PurgeAsync(id, concurrencyToken, cancellationToken);

        return
            await flagManager.SaveChangesAsync(cancellationToken) > 0
                ? purgedEntities.ToContracts()
                : throw new FlagPurgeFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Purge)}(id, concurrencyToken)", };
    }

    public async Task<IEnumerable<FlagResponse>> RestoreAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> restoredEntities = await flagManager.RestoreAsync(id, concurrencyToken, cancellationToken);

        return
            await flagManager.SaveChangesAsync(cancellationToken) > 0
                ? restoredEntities.ToContracts()
                : throw new FlagRestorationFailed { Area = $"{nameof(FlagCommandService)}.{nameof(Restore)}(id)", };
    }

    public Task EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.EnableAsync(id, cancellationToken);
    }

    public Task EnableAsync(string key, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.EnableAsync(key, cancellationToken);
    }

    public Task EnableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.EnableAsync(name, parentId, cancellationToken);
    }

    public Task DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.DisableAsync(id, cancellationToken);
    }

    public Task DisableAsync(string key, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.DisableAsync(key, cancellationToken);
    }

    public Task DisableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.DisableAsync(name, parentId, cancellationToken);
    }
}
