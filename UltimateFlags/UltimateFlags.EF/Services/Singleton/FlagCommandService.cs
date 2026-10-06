using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
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

namespace UltimateFlags.EF.Services.Singleton;

internal class FlagCommandService : IFlagCommandService
{
    private readonly ILogger<FlagCommandService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagCommandService(
        ILogger<FlagCommandService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _ultimateFlagConfiguration = options.Value;
    }

    public FlagResponse Create(FlagCreationRequest creationRequest)
    {
        _validateFlagName();

        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        string parentKey = _getParentKey(flagManager, creationRequest.ParentId);

        if (flagManager.Exists(creationRequest.Name, creationRequest.ParentId, deleted: null))
        {
            throw new FlagDuplicateFound { Area = $"{nameof(FlagService)}.{nameof(Create)}(contract)", };
        }

        Flag createdEntity = flagManager.Create(creationRequest.ToEntity(parentKey));

        return flagManager.SaveChanges() > 0
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
                    Area = $"{nameof(FlagService)}.{nameof(Create)}(contract)",
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
                : throw new FlagUpdateFailed { Area = $"{nameof(FlagService)}.{nameof(Update)}(id, contract)", };
    }

    public int ExecuteUpdate(Guid id, FlagUpdateRequest updateRequest)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.ExecuteUpdate(id, updateRequest);
    }

    public IEnumerable<FlagResponse> Delete(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> deletedEntities = flagManager.Delete(id);

        return
            flagManager.SaveChanges() > 0
                ? deletedEntities.ToContracts()
                : throw new FlagDeletionFailed { Area = $"{nameof(FlagService)}.{nameof(Delete)}(id)", };
    }

    public int ExecuteDelete(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.ExecuteDelete(id);
    }

    public IEnumerable<FlagResponse> Purge(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IEnumerable<Flag> purgedEntity = flagManager.Purge(id);

        return
            flagManager.SaveChanges() > 0
                ? purgedEntity.ToContracts()
                : throw new FlagPurgeFailed { Area = $"{nameof(FlagService)}.{nameof(Purge)}(id)", };
    }

    public int ExecutePurge(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.ExecutePurge(id);
    }

    public int ExecutePurge(DateTime? fromInclusive = null, DateTime? toInclusive = null)
    {
        _validateTimeRange();

        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.ExecutePurge(fromInclusive, toInclusive);

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
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Enable(id);
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

    public void Disable(string name, Guid? parentId)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        flagManager.Disable(name, parentId);
    }
}
