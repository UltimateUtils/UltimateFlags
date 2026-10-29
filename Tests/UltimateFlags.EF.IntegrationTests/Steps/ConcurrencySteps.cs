using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;
using UltimateFlags.EF.IntegrationTests.Utils;

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

    #region Given

    [Given("I have retrieved a flag, {Guid}")]
    public void HaveRetrievedAFlagWithTheId(Guid id)
    {
        FlagResponse? flag = _flagQueryService.Get(id);

        _scenarioContext.Set(flag, ContextKeys.RetrievedFlagKey);
    }

    #endregion Given

    #region When

    [When("I update the retrieved flag with the following properties")]
    public void UpdateTheRetrievedFlagWithTheFollowingProperties(DataTable table)
    {
        FlagResponse? retrievedFlag = _scenarioContext.Get<FlagResponse?>(ContextKeys.RetrievedFlagKey);

        Assert.That(retrievedFlag, Is.Not.Null);

        FlagUpdateRequest flagUpdateRequest = table.CreateInstance<FlagUpdateRequest>();

        try
        {
            FlagResponse updateResponse = _flagCommandService.Update(retrievedFlag.Id, flagUpdateRequest);
            _scenarioContext.Set(updateResponse, ContextKeys.UpdateResponseKey);
        }
        catch (Exception e)
        {
            _scenarioContext.Set(e, ContextKeys.ThrownExceptionKey);
        }
    }

    #endregion when

    #region Then

    [Then("ConcurrencyConflict exception is thrown")]
    public void ConcurrencyConflictExceptionIsThrown()
    {
        Exception x = _scenarioContext.Get<Exception>(ContextKeys.ThrownExceptionKey);

        Assert.That(x, Is.TypeOf<ConcurrencyConflict>());
    }

    #endregion Then
}
