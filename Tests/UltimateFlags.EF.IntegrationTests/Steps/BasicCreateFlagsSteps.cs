using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;
using UltimateFlags.EF.IntegrationTests.Utils;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class BasicCreateFlagsSteps
{
    private readonly ILogger<BasicFlagsSteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IConfiguration _configuration;

    private readonly IOptions<ServiceConfiguration> _options;

    public BasicCreateFlagsSteps(
        ILogger<BasicFlagsSteps> logger,
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
        _options = options;
    }

    [When("I create the following flag")]
    public void CreateTheFollowingFlag(DataTable table)
    {
        FlagCreationRequest flagToInsert = table.CreateInstance<FlagCreationRequest>();

        FlagResponse x = _flagCommandService.Create(flagToInsert);

        _scenarioContext.Set(x.Id, ContextKeys.NewlyAddedFlagIdKey);
    }

    [Then("Newly added flag is as follows")]
    public void TheNewlyAddedFlagIsReturnedCorrectly(DataTable table)
    {
        Flag expected = table.CreateInstance<Flag>();

        Guid id = _scenarioContext.Get<Guid>(ContextKeys.NewlyAddedFlagIdKey);

        FlagResponse? actual = _flagQueryService.Get(id);
        Assert.That(actual, Is.Not.Null);

        Assert.That(actual, Is.EqualTo(expected).Using(_FlagResponseFlagComparison()));
    }

    private static Func<FlagResponse, Flag, bool> _FlagResponseFlagComparison()
    {
        return
            (actual, expected) =>
                actual.Name == expected.Name
                && actual.Key == expected.Key
                && actual.IsOn == expected.IsOn
                && actual.Description == expected.Description
                && actual.ParentId == expected.ParentId;
    }
}
