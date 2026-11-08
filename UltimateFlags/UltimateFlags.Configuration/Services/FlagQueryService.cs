using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Configuration.Managers;
using UltimateFlags.Converters;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Configuration.Services;

internal class FlagQueryService : IFlagQueryService
{
    private readonly ILogger<FlagQueryService> _logger;

    private readonly IFlagManager _flagManager;

    public FlagQueryService(
        ILogger<FlagQueryService> logger,
        IFlagManager flagManager)
    {
        _logger = logger;
        _flagManager = flagManager;
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
                Area = $"{nameof(FlagQueryService)}.{nameof(GetRequired)}(id)",
            };

        return foundEntity.ToContract();
    }

    public FlagResponse? Get(string name, Guid? parentId)
    {
        return _flagManager.Read(name, parentId)?.ToContract();
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

    public IPagedList<FlagResponse> GetAllDeleted(int pageNumber = 1, int pageSize = 20)
    {
        throw new NotImplementedException();
    }

    public IPagedList<FlagResponse> List(
        string? searchString = null,
        bool? isOn = null,
        int pageNumber = 1,
        int pageSize = 20)
    {
        throw new NotImplementedException();
    }
}
