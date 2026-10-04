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
            throw new FlagDuplicateFound { Area = $"{nameof(FlagService)}.{nameof(Create)}(contract)", };
        }

        Flag createdEntity = _flagManager.Create(creationRequest.ToEntity(parentKey));

        return _flagManager.SaveChanges() > 0
            ? createdEntity.ToContract()
            : throw new FlagCreationFailed { Area = $"{nameof(FlagService)}.{nameof(Create)}(contract)", };

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
                    Area = $"{nameof(FlagService)}.{nameof(Create)}(contract)",
                };
            }

            return parent.Key;
        }
    }

    public FlagResponse Update(Guid id, FlagUpdateRequest updateRequest)
    {
        Flag updatedEntity = _flagManager.Update(id, updateRequest);

        return
            _flagManager.SaveChanges() > 0
                ? updatedEntity.ToContract()
                : throw new FlagUpdateFailed { Area = $"{nameof(FlagService)}.{nameof(Update)}(id, contract)", };
    }

    public int ExecuteUpdate(Guid id, FlagUpdateRequest updateRequest)
    {
        return _flagManager.ExecuteUpdate(id, updateRequest);
    }

    public IEnumerable<FlagResponse> Delete(Guid id)
    {
        IEnumerable<Flag> deletedEntities = _flagManager.Delete(id);

        return
            _flagManager.SaveChanges() > 0
                ? deletedEntities.ToContracts()
                : throw new FlagDeletionFailed { Area = $"{nameof(FlagService)}.{nameof(Delete)}(id)", };
    }

    public int ExecuteDelete(Guid id)
    {
        return _flagManager.ExecuteDelete(id);
    }

    public IEnumerable<FlagResponse> Purge(Guid id)
    {
        IEnumerable<Flag> purgedEntity = _flagManager.Purge(id);

        return
            _flagManager.SaveChanges() > 0
                ? purgedEntity.ToContracts()
                : throw new FlagPurgeFailed { Area = $"{nameof(FlagService)}.{nameof(Purge)}(id)", };
    }

    public int ExecutePurge(Guid id)
    {
        return _flagManager.ExecutePurge(id);
    }

    public int ExecutePurge(DateTime? fromInclusive = null, DateTime? toInclusive = null)
    {
        _validateTimeRange();

        return _flagManager.ExecutePurge(fromInclusive, toInclusive);

        void _validateTimeRange()
        {
            if (fromInclusive is null || toInclusive is null)
                return;

            if (fromInclusive.Value > toInclusive.Value)
            {
                throw new InvalidTimeRange
                {
                    Area = $"{nameof(FlagService)}.{nameof(ExecutePurge)}(from, to)",
                };
            }
        }
    }

    public void Enable(Guid id)
    {
        _flagManager.Enable(id);
    }

    public void Enable(string name, Guid? parentId)
    {
        _flagManager.Enable(name, parentId);
    }

    public void Disable(Guid id)
    {
        _flagManager.Disable(id);
    }

    public void Disable(string name, Guid? parentId)
    {
        _flagManager.Disable(name, parentId);
    }
}
