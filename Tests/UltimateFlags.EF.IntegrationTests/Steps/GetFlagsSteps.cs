using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;
using UltimateFlags.EF.IntegrationTests.Utils;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class GetFlagsSteps
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

    public GetFlagsSteps(
        ILogger<GetFlagsSteps> logger,
        FeatureContext featureContext,
        ScenarioContext scenarioContext,
        MyFlagDbContext flagDbContext,
        IFlagService flagService,
        IFlagQueryService flagQueryService,
        IFlagCommandService flagCommandService,
        IServiceScopeFactory serviceScopeFactory,
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
        _serviceScopeFactory = serviceScopeFactory;
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

    [When("I retrieve a flag by {string}")]
    public void RetrieveFlagByMyBuilding(string key)
    {
        FlagResponse? actual = _flagQueryService.Get(key);

        _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
    }

    [When("I retrieve all deleted flags")]
    public void RetrieveAllDeletedFlags()
    {
        FlagResponse[] deletedFlags = [.. _flagQueryService.GetAllDeleted()];

        _scenarioContext.Set(deletedFlags);
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

    [Then("the flags, {string}, are returned")]
    public void FlagsAreReturned(string expectedDeletedIdsResponse)
    {
        Guid[] expectedDeletedIds = [.. expectedDeletedIdsResponse.Split(',').Select(Guid.Parse)];

        FlagResponse[] actualDeletedFlags = _scenarioContext.Get<FlagResponse[]>();

        Assert.That(actualDeletedFlags.Select(f => f.Id), Is.EquivalentTo(expectedDeletedIds));
    }

    #endregion Then
}
