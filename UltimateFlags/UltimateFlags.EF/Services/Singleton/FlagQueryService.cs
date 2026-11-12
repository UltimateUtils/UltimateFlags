using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Converters;
using UltimateFlags.EF.Managers;
using UltimatePagination;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.Services.Singleton;

internal class FlagQueryService : IFlagQueryService
{
    private readonly ILogger<FlagQueryService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FlagQueryService(
        ILogger<FlagQueryService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public FlagResponse? Get(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.Read(id)?.ToContract();
    }

    public FlagResponse GetRequired(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

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
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.Read(name, parentId)?.ToContract();
    }

    public FlagResponse GetRequired(string name, Guid? parentId)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

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
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return flagManager.Read(key)?.ToContract();
    }

    public FlagResponse GetRequired(string key)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

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
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IPagedList<Flag> deletedEntities = flagManager.GetAllDeleted(pageNumber, pageSize);

        return deletedEntities.Convert(entity => entity.ToContract());
    }

    public IPagedList<FlagResponse> List(
        string? searchString = null,
        bool? isOn = null,
        int pageNumber = 1,
        int pageSize = 20)
    {
        if (!_IsValidPaginationInfo(pageNumber, pageSize))
        {
            throw new InvalidPaginationInfo
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(List)}(rootId, searchString, isOn, pageNumber, pageSize)",
            };
        }

        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IPagedList<Flag> foundEntities =
            flagManager
                .List(
                    searchString,
                    isOn,
                    pageNumber,
                    pageSize);

        return foundEntities.Convert(entity => entity.ToContract());
    }

    public async Task<FlagResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return (await flagManager.ReadAsync(id, cancellationToken: cancellationToken))?.ToContract();
    }

    public async Task<FlagResponse> GetRequiredAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag foundEntity =
            await flagManager.ReadAsync(id, cancellationToken: cancellationToken)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(GetRequiredAsync)}(id)",
            };

        return foundEntity.ToContract();
    }

    public async Task<FlagResponse?> GetAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return (await flagManager.ReadAsync(name, parentId, cancellationToken))?.ToContract();
    }

    public async Task<FlagResponse> GetRequiredAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag foundEntity =
            await flagManager.ReadAsync(name, parentId, cancellationToken)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(GetRequiredAsync)}(name, parentId)",
            };

        return foundEntity.ToContract();
    }

    public async Task<FlagResponse?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        return (await flagManager.ReadAsync(key, cancellationToken))?.ToContract();
    }

    public async Task<FlagResponse> GetRequiredAsync(string key, CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        Flag foundEntity =
            await flagManager.ReadAsync(key, cancellationToken)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(GetRequiredAsync)}(key)",
            };

        return foundEntity.ToContract();
    }

    public async Task<IPagedList<FlagResponse>> GetAllDeletedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IPagedList<Flag> deletedEntities = await flagManager.GetAllDeletedAsync(pageNumber, pageSize, cancellationToken);

        return deletedEntities.Convert(entity => entity.ToContract());
    }

    public async Task<IPagedList<FlagResponse>> ListAsync(
        string? searchString = null,
        bool? isOn = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (!_IsValidPaginationInfo(pageNumber, pageSize))
        {
            throw new InvalidPaginationInfo
            {
                Area = $"{nameof(FlagQueryService)}.{nameof(ListAsync)}(rootId, searchString, isOn, pageNumber, pageSize)",
            };
        }

        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagManager flagManager = serviceScope.ServiceProvider.GetRequiredService<IFlagManager>();

        IPagedList<Flag> foundEntities =
            await
                flagManager
                    .ListAsync(
                        searchString,
                        isOn,
                        pageNumber,
                        pageSize,
                        cancellationToken);

        return foundEntities.Convert(entity => entity.ToContract());
    }

    private static bool _IsValidPaginationInfo(int pageNumber, int pageSize)
    {
        return pageNumber > 0 && pageSize > 0;
    }
}
