using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Config;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class EnableDisableFlagsSteps
{
    private readonly ILogger<ConcurrencySteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly IConfiguration _configuration;

    private readonly ServiceConfiguration _serviceConfiguration;

    public EnableDisableFlagsSteps(
        ILogger<ConcurrencySteps> logger,
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

    [Given("the flag '{Guid}' has been disabled")]
    public void FlagHasBeenDisabled(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagCommandService flagCommandService = serviceScope.ServiceProvider.GetRequiredService<IFlagCommandService>();

        flagCommandService.Disable(id);
    }

    [Given("the flag with {string} and {Guid} has been disabled")]
    public void FlagWithNameAndParentIdHasBeenDisabled(string name, Guid? parentId)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagCommandService flagCommandService = serviceScope.ServiceProvider.GetRequiredService<IFlagCommandService>();

        flagCommandService.Disable(name, parentId);
    }

    #endregion Given

    #region When

    [When("I enable a flag '{Guid}'")]
    public void EnableFlag(Guid id)
    {
        _flagCommandService.Enable(id);
    }

    [When("I disable a flag '{Guid}'")]
    public void DisableFlag(Guid id)
    {
        _flagCommandService.Disable(id);
    }

    [When("I enable a flag with {string} and {Guid}")]
    public void EnableFlagWithNameAndParentId(string name, Guid? parentId)
    {
        _flagCommandService.Enable(name, parentId);
    }

    [When("I disable a flag with {string} and {Guid}")]
    public void DisableFlagWithNameAndParentId(string name, Guid? parentId)
    {
        _flagCommandService.Disable(name, parentId);
    }

    [When("I disable a flag with {string}")]
    public void DisableFlagWithKey(string key)
    {
        _flagCommandService.Disable(key);
    }

    #endregion When

    #region Then

    [Then("the flag '{Guid}' is enabled")]
    public void FlagIsEnabled(Guid id)
    {
        FlagResponse? flag = _flagQueryService.Get(id);

        Assert.That(flag, Is.Not.Null);
        Assert.That(flag.IsOn, Is.True);
    }

    [Then("the flag '{Guid}' is disabled")]
    public void FlagIsDisabled(Guid id)
    {
        FlagResponse? flag = _flagQueryService.Get(id);

        Assert.That(flag, Is.Not.Null);
        Assert.That(flag.IsOn, Is.False);
    }

    [Then("the flag with {string} and {Guid} is enabled")]
    public void FlagIsEnabled(string name, Guid? parentId)
    {
        FlagResponse? flag = _flagQueryService.Get(name, parentId);

        Assert.That(flag, Is.Not.Null);
        Assert.That(flag.IsOn, Is.True);
    }

    [Then("the flag with {string} and {Guid} is disabled")]
    public void FlagIsDisabled(string name, Guid? parentId)
    {
        FlagResponse? flag = _flagQueryService.Get(name, parentId);

        Assert.That(flag, Is.Not.Null);
        Assert.That(flag.IsOn, Is.False);
    }

    [Then("the flag with {string} is disabled")]
    public void FlagIsDisabled(string key)
    {
        FlagResponse? flag = _flagQueryService.Get(key);

        Assert.That(flag, Is.Not.Null);
        Assert.That(flag.IsOn, Is.False);
    }

    #endregion Then
}
