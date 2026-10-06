using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Managers;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Services.Singleton;

internal class FlagQueryService : IFlagQueryService
{
    private readonly ILogger<FlagQueryService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public FlagQueryService(
        ILogger<FlagQueryService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public FlagResponse? Get(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public FlagResponse GetRequired(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public FlagResponse? Get(string name, Guid? parentId)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public FlagResponse GetRequired(string name, Guid? parentId)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public FlagResponse? Get(string key)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public FlagResponse GetRequired(string key)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }

    public IPagedList<FlagResponse> List(string? searchString = null, bool? isOn = null, int pageNumber = 1, int pageSize = 20)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        throw new NotImplementedException();
    }
}
