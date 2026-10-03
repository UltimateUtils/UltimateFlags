using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Managers;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Services;

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
}
