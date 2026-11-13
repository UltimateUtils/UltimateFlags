using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using TestsBase.Utils;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Config;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class IsOnSteps
{
    private readonly ILogger<GetFlagsSteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly IConfiguration _configuration;

    private readonly ServiceConfiguration _serviceConfiguration;

    public IsOnSteps(
        ILogger<GetFlagsSteps> logger,
        FeatureContext featureContext,
        ScenarioContext scenarioContext,
        MyFlagDbContext flagDbContext,
        IFlagService flagService,
        IFlagQueryService flagQueryService,
        IFlagCommandService flagCommandService,
        IServiceScopeFactory serviceScopeFactory,
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
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _serviceConfiguration = serviceConfiguration;
    }

    #region Given

    [Given("I have turned off the following flags")]
    public void HaveTurnedOffFollowingFlags(DataTable table)
    {
        IEnumerable<GuidIdObject> flagsToTurnOff = table.CreateSet<GuidIdObject>();

        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagCommandService flagCommandService = serviceScope.ServiceProvider.GetRequiredService<IFlagCommandService>();

        foreach (GuidIdObject idToTurnOff in flagsToTurnOff)
        {
            flagCommandService.Disable(idToTurnOff.Id);
        }
    }

    #endregion Given

    #region When

    [When("I call IsOn by key, {string}")]
    public void CallIsOnByKeyMyBuilding(string key)
    {
        bool actual = _flagService.IsOn(key);

        _scenarioContext.Set(actual);
    }

    [When("I call async IsOn by key, {string}")]
    public async Task CallAsyncIsOnByKeyMyBuildingAsync(string key)
    {
        bool actual = await _flagService.IsOnAsync(key);

        _scenarioContext.Set(actual);
    }

    #endregion When

    #region Then

    [Then("the response is {Boolean}")]
    public void ResponseIsTrue(bool expected)
    {
        bool actual = _scenarioContext.Get<bool>();

        Assert.That(actual, Is.EqualTo(expected));
    }

    #endregion Then
}
