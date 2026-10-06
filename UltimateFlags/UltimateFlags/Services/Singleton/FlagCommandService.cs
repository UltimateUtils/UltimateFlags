using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Managers;

namespace UltimateFlags.Services.Singleton;

internal class FlagCommandService : IFlagCommandService
{
    private readonly ILogger<FlagCommandService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public FlagCommandService(
        ILogger<FlagCommandService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public FlagResponse Create(FlagCreationRequest creationRequest)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public FlagResponse Update(Guid id, FlagUpdateRequest updateRequest)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public int ExecuteUpdate(Guid id, FlagUpdateRequest updateRequest)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public IEnumerable<FlagResponse> Delete(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public int ExecuteDelete(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public IEnumerable<FlagResponse> Purge(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public int ExecutePurge(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public int ExecutePurge(DateTime? fromInclusive = null, DateTime? toInclusive = null)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public void Enable(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public void Enable(string name, Guid? parentId)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public void Disable(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public void Disable(string name, Guid? parentId)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }
}
