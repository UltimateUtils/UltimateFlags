using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Managers;

namespace UltimateFlags.Services;

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
