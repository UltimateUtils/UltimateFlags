using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Managers;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Services;

internal class FlagManagementService : IFlagManagementService
{
    private readonly ILogger<FlagManagementService> _logger;

    private readonly IFlagManager _flagManager;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagManagementService(
        ILogger<FlagManagementService> logger,
        IFlagManager flagManager,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _flagManager = flagManager;
        _ultimateFlagConfiguration = options.Value;
    }

    public FlagResponse Create(FlagCreationRequest creationRequest)
    {
        throw new NotImplementedException();
    }

    public FlagResponse? Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public FlagResponse GetRequired(Guid id)
    {
        throw new NotImplementedException();
    }

    public FlagResponse? Get(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public FlagResponse GetRequired(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public FlagResponse? Get(string key)
    {
        throw new NotImplementedException();
    }

    public FlagResponse GetRequired(string key)
    {
        throw new NotImplementedException();
    }

    public IPagedList<FlagResponse> List(string? searchString = null, bool? isOn = null, int pageNumber = 1, int pageSize = 20)
    {
        throw new NotImplementedException();
    }

    public FlagResponse Update(Guid id, FlagUpdateRequest updateRequest)
    {
        throw new NotImplementedException();
    }

    public int ExecuteUpdate(Guid id, FlagUpdateRequest updateRequest)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<FlagResponse> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public int ExecuteDelete(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<FlagResponse> Purge(Guid id)
    {
        throw new NotImplementedException();
    }

    public int ExecutePurge(Guid id)
    {
        throw new NotImplementedException();
    }

    public int ExecutePurge(DateTime? fromInclusive = null, DateTime? toInclusive = null)
    {
        throw new NotImplementedException();
    }

    public void Enable(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Enable(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public void Disable(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Disable(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }
}
