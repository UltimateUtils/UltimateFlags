using Microsoft.AspNetCore.Mvc;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Api.v10.Controllers;

[ApiController]
[Route("flags")]
public class FlagsController : ControllerBase
{
    private readonly ILogger<FlagsController> _logger;

    private readonly IFlagService _flagService;

    private readonly IFlagManagementService _flagManagementService;

    public FlagsController(
        ILogger<FlagsController> logger,
        IFlagService flagService,
        IFlagManagementService flagManagementService)
    {
        _logger = logger;
        _flagService = flagService;
        _flagManagementService = flagManagementService;
    }

    [HttpPost]
    [Route("")]
    public FlagResponse Create(FlagCreationRequest contract)
    {
        return _flagManagementService.Create(contract);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public FlagResponse GetById([FromRoute] Guid id)
    {
        return _flagManagementService.GetRequired(id);
    }

    [HttpGet]
    [Route("")]
    public FlagResponse Get([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        return _flagManagementService.GetRequired(name, parentId);
    }

    [HttpGet]
    [Route("{key}")]
    public FlagResponse Get([FromRoute] string key)
    {
        return _flagManagementService.GetRequired(key);
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
            _flagManagementService
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
        return _flagManagementService.Update(id, contract);
    }

    [HttpPut]
    [Route("execute-update/{id:guid}")]
    public int ExecuteUpdate([FromRoute] Guid id, [FromBody] FlagUpdateRequest contract)
    {
        return _flagManagementService.ExecuteUpdate(id, contract);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IEnumerable<FlagResponse> Delete([FromRoute] Guid id, [FromQuery] bool purge = false)
    {
        return
            purge
                ? _flagManagementService.Purge(id)
                : _flagManagementService.Delete(id);
    }

    [HttpDelete]
    [Route("execute-delete/{id:guid}")]
    public int ExecuteDelete([FromRoute] Guid id, [FromQuery] bool purge = false)
    {
        return
            purge
                ? _flagManagementService.ExecutePurge(id)
                : _flagManagementService.ExecuteDelete(id);
    }

    [HttpDelete]
    [Route("execute-purge")]
    public int ExecutePurge([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return _flagManagementService.ExecutePurge(from, to);
    }

    [HttpPut]
    [Route("{id:guid}/enable")]
    public void EnableById([FromRoute] Guid id)
    {
        _flagManagementService.Enable(id);
    }

    [HttpPut]
    [Route("enable")]
    public void EnableByName([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        _flagManagementService.Enable(name, parentId);
    }

    [HttpPut]
    [Route("{id:guid}/disable")]
    public void DisableById([FromRoute] Guid id)
    {
        _flagManagementService.Disable(id);
    }

    [HttpPut]
    [Route("disable")]
    public void DisableByName([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        _flagManagementService.Disable(name, parentId);
    }

    [HttpGet]
    [Route("{key}/is-on")]
    public bool IsOnByKey([FromRoute] string key)
    {
        return _flagService.IsOn(key);
    }

    [HttpGet]
    [Route("{id:guid}/is-on")]
    public bool IsOnByKey([FromRoute] Guid id)
    {
        return _flagService.IsOn(id);
    }
}
