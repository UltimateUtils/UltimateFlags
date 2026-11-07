using Microsoft.AspNetCore.Mvc;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;

namespace UltimateFlags.Configuration.Api.Controllers;

[ApiController]
[Route("flags")]
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

    [HttpDelete]
    [Route("{id:guid}")]
    public IEnumerable<FlagResponse> Delete([FromRoute] Guid id)
    {
        _logger.LogDebug("Deleting flag. (id: {id})", id);

        return _flagCommandService.Delete(id);
    }

    [HttpDelete]
    [Route("/purge/{id:guid}")]
    public IEnumerable<FlagResponse> Purge([FromRoute] Guid id)
    {
        _logger.LogDebug("Purging flag. (id: {id})", id);

        return _flagCommandService.Purge(id);
    }

    [HttpPost]
    [Route("restore/{id:guid}")]
    public IEnumerable<FlagResponse> Restore(Guid id)
    {
        _logger.LogDebug("Restoring flag. (id: {id})", id);

        return _flagCommandService.Restore(id);
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
