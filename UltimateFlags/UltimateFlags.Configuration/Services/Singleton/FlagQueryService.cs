using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Configuration.Managers;
using UltimateFlags.Converters;
using UltimatePagination;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Configuration.Services.Singleton;

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

        return flagManager.Read(id)?.ToContract();
    }

    public FlagResponse GetRequired(Guid id)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag foundEntity =
            flagManager.Read(id)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(GetRequired)}(id)",
            };

        return foundEntity.ToContract();
    }

    public FlagResponse? Get(string name, Guid? parentId)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.Read(name, parentId)?.ToContract();
    }

    public FlagResponse GetRequired(string name, Guid? parentId)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag foundEntity =
            flagManager.Read(name, parentId)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(GetRequired)}(name, parentId)",
            };

        return foundEntity.ToContract();
    }

    public FlagResponse? Get(string key)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.Read(key)?.ToContract();
    }

    public FlagResponse GetRequired(string key)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag foundEntity =
            flagManager.Read(key)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(GetRequired)}(key)",
            };

        return foundEntity.ToContract();
    }

    public IPagedList<FlagResponse> GetAllDeleted(int pageNumber = 1, int pageSize = 20)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        IPagedList<Flag> deletedEntities = flagManager.GetAllDeleted(pageNumber, pageSize);

        return deletedEntities.Convert(entity => entity.ToContract());
    }

    public IPagedList<FlagResponse> List(
        string? searchString = null,
        bool? isOn = null,
        int pageNumber = 1,
        int pageSize = 20)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IFlagManager flagManager = scope.ServiceProvider.GetRequiredService<IFlagManager>();

        if (!_IsValidPaginationInfo(pageNumber, pageSize))
        {
            throw new InvalidPaginationInfo
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(List)}(rootId, searchString, isOn, pageNumber, pageSize)",
            };
        }

        IPagedList<Flag> foundEntities =
            flagManager
                .List(
                    searchString,
                    isOn,
                    pageNumber,
                    pageSize);

        return foundEntities.Convert(entity => entity.ToContract());
    }

    public Task<FlagResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FlagResponse> GetRequiredAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FlagResponse?> GetAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FlagResponse> GetRequiredAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FlagResponse?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FlagResponse> GetRequiredAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<FlagResponse>> GetAllDeletedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<FlagResponse>> ListAsync(
        string? searchString = null,
        bool? isOn = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static bool _IsValidPaginationInfo(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return pageNumber > 0 && pageSize > 0;
    }
}
