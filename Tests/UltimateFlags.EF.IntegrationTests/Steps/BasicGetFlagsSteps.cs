using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;
using UltimateFlags.EF.IntegrationTests.Utils;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class BasicGetFlagsSteps
{
    private readonly ILogger<BasicGetFlagsSteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IConfiguration _configuration;

    private readonly ServiceConfiguration _serviceConfiguration;

    public BasicGetFlagsSteps(
        ILogger<BasicGetFlagsSteps> logger,
        FeatureContext featureContext,
        ScenarioContext scenarioContext,
        MyFlagDbContext flagDbContext,
        IFlagService flagService,
        IFlagQueryService flagQueryService,
        IFlagCommandService flagCommandService,
        IConfiguration configuration,
        IOptions<ServiceConfiguration> options)
    {
        _logger = logger;
        _featureContext = featureContext;
        _scenarioContext = scenarioContext;
        _flagDbContext = flagDbContext;
        _flagService = flagService;
        _flagQueryService = flagQueryService;
        _flagCommandService = flagCommandService;
        _configuration = configuration;
        _serviceConfiguration = options.Value;
    }

    #region When

    [When("retrieve all flags")]
    public void RetrieveAllFlags()
    {
        FlagResponse[] actualFlags = [.. _flagQueryService.List(pageSize: int.MaxValue)];

        _scenarioContext.Set(actualFlags, ContextKeys.AllFlagsKey);
    }

    [When("retrieve a flag by {Guid}")]
    public void RetrieveFlagBy(Guid id)
    {
        FlagResponse? actual = _flagQueryService.Get(id);

        _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
    }

    [When("I retrieve a flag by {string} and {Guid}")]
    public void RetrieveFlagBy(string name, Guid? parentId)
    {
        FlagResponse? actual = _flagQueryService.Get(name, parentId);

        _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
    }

    [When("get a required flag by {Guid}")]
    public void GetRequiredFlagById(Guid id)
    {
        try
        {
            FlagResponse actual = _flagQueryService.GetRequired(id);
            _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
        }
        catch (Exception e)
        {
            _scenarioContext.Set(e, ContextKeys.ThrownExceptionKey);
        }
    }

    [When("I get a required flag by {string} and {Guid}")]
    public void GetRequiredFlagByName(string name, Guid? parentId)
    {
        try
        {
            FlagResponse actual = _flagQueryService.GetRequired(name, parentId);
            _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
        }
        catch (Exception e)
        {
            _scenarioContext.Set(e, ContextKeys.ThrownExceptionKey);
        }
    }

    #endregion When

    #region Then

    [Then("all the following flags are returned correctly")]
    public void AllFlagsAreReturnedCorrectly(DataTable table)
    {
        IEnumerable<FlagResponse> expectedFlags = table.CreateSet<FlagResponse>();
        FlagResponse[] actualFlags = _scenarioContext.Get<FlagResponse[]>(ContextKeys.AllFlagsKey);

        Assert.That(actualFlags, Is.EquivalentTo(expectedFlags));
    }

    [Then("the following flag is returned correctly")]
    public void TheFollowingFlagIsReturnedCorrectly(DataTable table)
    {
        FlagResponse expectedFlag = table.CreateInstance<FlagResponse>();
        FlagResponse? actual = _scenarioContext.Get<FlagResponse?>(ContextKeys.RetrievedFlagKey);

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(expectedFlag));
    }

    [Then("null is returned")]
    public void ThenNullIsReturned()
    {
        FlagResponse? actual = _scenarioContext.Get<FlagResponse?>(ContextKeys.RetrievedFlagKey);

        Assert.That(actual, Is.Null);
    }

    [Then("FlagNotFound exception is thrown")]
    public void ThenFlagNotFoundExceptionIsThrown()
    {
        Exception x = _scenarioContext.Get<Exception>(ContextKeys.ThrownExceptionKey);

        Assert.That(x, Is.TypeOf<FlagNotFound>());
    }

    #endregion Then
}
