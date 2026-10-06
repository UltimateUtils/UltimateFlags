using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
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

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagQueryService(
        ILogger<FlagQueryService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _ultimateFlagConfiguration = options.Value;
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
                Area = $"{nameof(FlagService)}.{nameof(GetRequired)}(id)",
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
                Area = $"{nameof(FlagService)}.{nameof(GetRequired)}(name, parentId)",
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
                Area = $"{nameof(FlagService)}.{nameof(GetRequired)}(key)",
            };

        return foundEntity.ToContract();
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
                Area = $"{nameof(FlagService)}.{nameof(List)}(searchString, isOn, pageNumber, pageSize)",
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

    private static bool _IsValidPaginationInfo(int pageNumber, int pageSize)
    {
        return pageNumber > 0 && pageSize > 0;
    }
}
