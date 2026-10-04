using Microsoft.AspNetCore.Mvc;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.Api.Controllers;

[ApiController]
[Route("flags")]
public class FlagsController : ControllerBase
{
    private readonly ILogger<FlagsController> _logger;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    public FlagsController(
        ILogger<FlagsController> logger,
        IFlagService flagService,
        IFlagQueryService flagQueryService)
    {
        _logger = logger;
        _flagService = flagService;
        _flagQueryService = flagQueryService;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public FlagResponse GetById([FromRoute] Guid id)
    {
        _logger.LogDebug("Getting flag with id: {id}", id);

        return _flagQueryService.GetRequired(id);
    }

    [HttpGet]
    [Route("")]
    public FlagResponse Get([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        _logger.LogDebug("Getting flag with name: {name}, parentId: {parentId}", name, parentId);

        return _flagQueryService.GetRequired(name, parentId);
    }

    [HttpGet]
    [Route("{key}")]
    public FlagResponse Get([FromRoute] string key)
    {
        _logger.LogDebug("Getting flag with key: {key}", key);

        return _flagQueryService.GetRequired(key);
    }

    [HttpGet]
    [Route("list")]
    public IPagedList<FlagResponse> List(
        [FromQuery] string? searchString = null,
        [FromQuery] bool? isOn = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        _logger.LogDebug(
            "Listing flags with searchString: {searchString}, isOn: {isOn}, pageNumber: {pageNumber}, pageSize: {pageSize}",
            searchString,
            isOn,
            pageNumber,
            pageSize);

        return
            _flagQueryService
                .List(
                    searchString,
                    isOn,
                    pageNumber,
                    pageSize);
    }

    [HttpGet]
    [Route("{key}/is-on")]
    public bool IsOnByKey([FromRoute] string key)
    {
        _logger.LogDebug("Checking flag with key: {key}", key);

        return _flagService.IsOn(key);
    }
}
