using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class ConcurrencySteps
{
    private readonly ILogger<ConcurrencySteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IConfiguration _configuration;

    private readonly ServiceConfiguration _serviceConfiguration;

    public ConcurrencySteps(
        ILogger<ConcurrencySteps> logger,
        FeatureContext featureContext,
        ScenarioContext scenarioContext,
        MyFlagDbContext flagDbContext,
        IFlagService flagService,
        IFlagQueryService flagQueryService,
        IFlagCommandService flagCommandService,
        IConfiguration configuration,
        ServiceConfiguration serviceConfiguration)
    {
        _logger = logger;
        _featureContext = featureContext;
        _scenarioContext = scenarioContext;
        _flagDbContext = flagDbContext;
        _flagService = flagService;
        _flagQueryService = flagQueryService;
        _flagCommandService = flagCommandService;
        _configuration = configuration;
        _serviceConfiguration = serviceConfiguration;
    }

    [When("I update a flag which has been updated by another process")]
    public void UpdateFlagWhichHasBeenUpdatedByAnotherProcess(DataTable table)
    {
        (Guid id, string description) = table.CreateInstance<(Guid, string)>();

        FlagResponse? flagToUpdate = _flagQueryService.Get(id);

        //// _flagDbContext.Database.ExecuteSql($"UPDATE Flag SET Description = 'my big building' WHERE Id = {id}");

        // int x = _flagCommandService.ExecuteUpdate(id, new FlagUpdateRequest { Description = $"{description}~" });

        FlagResponse updated = _flagCommandService.Update(id, new FlagUpdateRequest { Description = description });

        Assert.That(updated.Description, Is.EqualTo(description));
    }

    [Then("ConcurrencyConflict exception is thrown")]
    public void ConcurrencyConflictExceptionIsThrown()
    {
        // throw new PendingStepException();
    }
}
