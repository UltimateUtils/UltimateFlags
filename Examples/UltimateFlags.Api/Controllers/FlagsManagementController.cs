using Microsoft.AspNetCore.Mvc;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;

namespace UltimateFlags.Api.Controllers;

[ApiController]
[Route("flags-management")]
public class FlagsManagementController : ControllerBase
{
    private readonly ILogger<FlagsManagementController> _logger;

    private readonly IFlagCommandService _flagCommandService;

    public FlagsManagementController(
        ILogger<FlagsManagementController> logger,
        IFlagCommandService flagCommandService)
    {
        _logger = logger;
        _flagCommandService = flagCommandService;
    }

    [HttpPost]
    [Route("")]
    public FlagResponse Create(FlagCreationRequest contract)
    {
        _logger.LogDebug("Creating flag {contract}", contract);

        return _flagCommandService.Create(contract);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public FlagResponse Update([FromRoute] Guid id, [FromBody] FlagUpdateRequest contract)
    {
        _logger.LogDebug("Updating flag. (id: {id}, {contract})", id, contract);

        return _flagCommandService.Update(id, contract);
    }

    [HttpPut]
    [Route("execute-update/{id:guid}")]
    public int ExecuteUpdate([FromRoute] Guid id, [FromBody] FlagUpdateRequest contract)
    {
        _logger.LogDebug("Executing update. (id: {id}, {contract})", id, contract);

        return _flagCommandService.ExecuteUpdate(id, contract);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IEnumerable<FlagResponse> Delete([FromRoute] Guid id)
    {
        _logger.LogDebug("Deleting flag. (id: {id})", id);

        return _flagCommandService.Delete(id);
    }

    [HttpDelete]
    [Route("execute-delete/{id:guid}")]
    public int ExecuteDelete([FromRoute] Guid id)
    {
        _logger.LogDebug("Executing delete. (id: {id})", id);

        return _flagCommandService.ExecuteDelete(id);
    }

    [HttpDelete]
    [Route("/purge/{id:guid}")]
    public IEnumerable<FlagResponse> Purge([FromRoute] Guid id)
    {
        _logger.LogDebug("Purging flag. (id: {id})", id);

        return _flagCommandService.Purge(id);
    }

    [HttpDelete]
    [Route("execute-purge/{id:guid}")]
    public int ExecutePurge([FromRoute] Guid id)
    {
        _logger.LogDebug("Executing purge. (id: {id})", id);

        return _flagCommandService.ExecutePurge(id);
    }

    [HttpDelete]
    [Route("execute-purge")]
    public int ExecutePurge([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        _logger.LogDebug("Executing purge. (from: {from}, to: {to})", from, to);

        return _flagCommandService.ExecutePurge(from, to);
    }

    [HttpPut]
    [Route("{id:guid}/enable")]
    public void EnableById([FromRoute] Guid id)
    {
        _logger.LogDebug("Enabling flag. (id: {id})", id);

        _flagCommandService.Enable(id);
    }

    [HttpPut]
    [Route("enable")]
    public void EnableByName([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        _logger.LogDebug("Enabling flag. (name: {name}, parentId: {parentId})", name, parentId);

        _flagCommandService.Enable(name, parentId);
    }

    [HttpPut]
    [Route("{id:guid}/disable")]
    public void DisableById([FromRoute] Guid id)
    {
        _logger.LogDebug("Disabling flag. (id: {id})", id);

        _flagCommandService.Disable(id);
    }

    [HttpPut]
    [Route("disable")]
    public void DisableByName([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        _logger.LogDebug("Disabling flag. (name: {name}, parentId: {parentId})", name, parentId);

        _flagCommandService.Disable(name, parentId);
    }
}
