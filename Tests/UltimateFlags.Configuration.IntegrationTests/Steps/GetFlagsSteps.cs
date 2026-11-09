using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using TestsBase.Utils;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Configuration.IntegrationTests.Config;

namespace UltimateFlags.Configuration.IntegrationTests.Steps;

[Binding]
public class GetFlagsSteps
{
    private readonly ILogger<GetFlagsSteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

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
        _flagService = flagService;
        _flagQueryService = flagQueryService;
        _flagCommandService = flagCommandService;
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _serviceConfiguration = serviceConfiguration;
    }

    #region When

    [When("retrieve a flag by {Guid}")]
    public void RetrieveFlagById(Guid id)
    {
        FlagResponse? actual = _flagQueryService.Get(id);

        _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
    }

    [When("I retrieve a flag by {string} and {Guid}")]
    public void RetrieveFlagByAndParentId(string name, Guid? parentId)
    {
        FlagResponse? actual = _flagQueryService.Get(name, parentId);

        _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
    }

    [When("I retrieve a flag by {string}")]
    public void RetrieveFlagByKey(string key)
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

    [Then("the following flag is returned correctly")]
    public void FollowingFlagIsReturnedCorrectly(DataTable table)
    {
        FlagResponse expectedFlag = table.CreateInstance<FlagResponse>();
        FlagResponse? actual = _scenarioContext.Get<FlagResponse?>(ContextKeys.RetrievedFlagKey);

        if (expectedFlag.DeletedAt.HasValue)
        {
            Assert.That(actual, Is.Null);
        }
        else
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(expectedFlag).Using<FlagResponse>(_withOutConcurrencyToken()));
        }
    }

    private static Func<FlagResponse, FlagResponse, bool> _withOutConcurrencyToken()
    {
        return
            (actual, expected) =>
                actual.Id == expected.Id
                && actual.ParentId == expected.ParentId
                && actual.IsOn == expected.IsOn
                && actual.Name == expected.Name
                && actual.Key == expected.Key
                && actual.Description == expected.Description
                && actual.CreatedAt == expected.CreatedAt
                && actual.UpdatedAt == expected.UpdatedAt
                && actual.DeletedAt == expected.DeletedAt;
    }

    [Then("null is returned")]
    public void NullIsReturned()
    {
        FlagResponse? actual = _scenarioContext.Get<FlagResponse?>(ContextKeys.RetrievedFlagKey);

        Assert.That(actual, Is.Null);
    }

    [Then("the following flags are returned")]
    public void FlagsAreReturned(IEnumerable<Guid?> ids)
    {
        FlagResponse[] actual = _scenarioContext.Get<FlagResponse[]>();
        Assert.That(actual.Select(f => f.Id), Is.EquivalentTo(ids));
    }

    #endregion Then
}
