using Microsoft.AspNetCore.Mvc;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Api.Controllers;

[ApiController]
[Route("flags")]
public class FlagsController : ControllerBase
{
    private readonly ILogger<FlagsController> _logger;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    public FlagsController(
        ILogger<FlagsController> logger,
        IFlagService flagService,
        IFlagQueryService flagQueryService,
        IFlagCommandService flagCommandService)
    {
        _logger = logger;
        _flagService = flagService;
        _flagQueryService = flagQueryService;
        _flagCommandService = flagCommandService;
    }

    [HttpPost]
    [Route("")]
    public FlagResponse Create(FlagCreationRequest contract)
    {
        return _flagCommandService.Create(contract);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public FlagResponse GetById([FromRoute] Guid id)
    {
        return _flagQueryService.GetRequired(id);
    }

    [HttpGet]
    [Route("")]
    public FlagResponse Get([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        return _flagQueryService.GetRequired(name, parentId);
    }

    [HttpGet]
    [Route("{key}")]
    public FlagResponse Get([FromRoute] string key)
    {
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
        return
            _flagQueryService
                .List(
                    searchString,
                    isOn,
                    pageNumber,
                    pageSize);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public FlagResponse Update([FromRoute] Guid id, [FromBody] FlagUpdateRequest contract)
    {
        return _flagCommandService.Update(id, contract);
    }

    [HttpPut]
    [Route("execute-update/{id:guid}")]
    public int ExecuteUpdate([FromRoute] Guid id, [FromBody] FlagUpdateRequest contract)
    {
        return _flagCommandService.ExecuteUpdate(id, contract);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IEnumerable<FlagResponse> Delete([FromRoute] Guid id, [FromQuery] bool purge = false)
    {
        return
            purge
                ? _flagCommandService.Purge(id)
                : _flagCommandService.Delete(id);
    }

    [HttpDelete]
    [Route("execute-delete/{id:guid}")]
    public int ExecuteDelete([FromRoute] Guid id, [FromQuery] bool purge = false)
    {
        return
            purge
                ? _flagCommandService.ExecutePurge(id)
                : _flagCommandService.ExecuteDelete(id);
    }

    [HttpDelete]
    [Route("execute-purge")]
    public int ExecutePurge([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return _flagCommandService.ExecutePurge(from, to);
    }

    [HttpPut]
    [Route("{id:guid}/enable")]
    public void EnableById([FromRoute] Guid id)
    {
        _flagCommandService.Enable(id);
    }

    [HttpPut]
    [Route("enable")]
    public void EnableByName([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        _flagCommandService.Enable(name, parentId);
    }

    [HttpPut]
    [Route("{id:guid}/disable")]
    public void DisableById([FromRoute] Guid id)
    {
        _flagCommandService.Disable(id);
    }

    [HttpPut]
    [Route("disable")]
    public void DisableByName([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        _flagCommandService.Disable(name, parentId);
    }

    [HttpGet]
    [Route("{key}/is-on")]
    public bool IsOnByKey([FromRoute] string key)
    {
        return _flagService.IsOn(key);
    }
}
