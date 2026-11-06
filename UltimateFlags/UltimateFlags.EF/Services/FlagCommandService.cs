using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
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

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagCommandService(
        ILogger<FlagCommandService> logger,
        IFlagManager flagManager,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _flagManager = flagManager;
        _ultimateFlagConfiguration = options.Value;
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
}
