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

namespace UltimateFlags.EF.Services;

internal class FlagQueryService : IFlagQueryService
{
    private readonly ILogger<FlagQueryService> _logger;

    private readonly IFlagManager _flagManager;

    private readonly UltimateFlagConfiguration _ultimateFlagConfiguration;

    public FlagQueryService(
        ILogger<FlagQueryService> logger,
        IFlagManager flagManager,
        IOptions<UltimateFlagConfiguration> options)
    {
        _logger = logger;
        _flagManager = flagManager;
        _ultimateFlagConfiguration = options.Value;
    }

    public FlagResponse? Get(Guid id)
    {
        return _flagManager.Read(id)?.ToContract();
    }

    public FlagResponse GetRequired(Guid id)
    {
        Flag foundEntity =
            _flagManager.Read(id)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagService)}.{nameof(GetRequired)}(id)",
            };

        return foundEntity.ToContract();
    }

    public FlagResponse? Get(string name, Guid? parentId)
    {
        return _flagManager.Read(name, parentId)?.ToContract();
    }

    public FlagResponse GetRequired(string name, Guid? parentId)
    {
        Flag foundEntity =
            _flagManager.Read(name, parentId)
            ?? throw new FlagNotFound
            {
                Area = $"{nameof(FlagService)}.{nameof(GetRequired)}(name, parentId)",
            };

        return foundEntity.ToContract();
    }

    public FlagResponse? Get(string key)
    {
        return _flagManager.Read(key)?.ToContract();
    }

    public FlagResponse GetRequired(string key)
    {
        Flag foundEntity =
            _flagManager.Read(key)
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

        IPagedList<Flag> foundEntities =
            _flagManager
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
