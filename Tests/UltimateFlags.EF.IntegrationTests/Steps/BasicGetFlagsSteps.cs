using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class BasicGetFlagsSteps
{
    private const string AllFlagsKey = "AllFlags";

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

    [Given("DB has the following flags")]
    public async Task DbHasTheFollowingFlagsAsync(DataTable table)
    {
        IEnumerable<Flag> flags = table.CreateSet<Flag>();

        await _flagDbContext.Flags.AddRangeAsync(flags);

        _ = await _flagDbContext.SaveChangesAsync();
    }

    [When("retrieve all flags")]
    public void RetrieveAllFlags()
    {
        FlagResponse[] actualFlags = [.. _flagQueryService.List(pageSize: int.MaxValue)];

        _scenarioContext[AllFlagsKey] = actualFlags;
    }

    [Then("all the following flags are returned correctly")]
    public void AllFlagsAreReturnedCorrectly(DataTable table)
    {
        IEnumerable<FlagResponse> expectedFlags = table.CreateSet<FlagResponse>();
        FlagResponse[]? actualFlags = _scenarioContext[AllFlagsKey] as FlagResponse[];

        Assert.That(actualFlags, Is.Not.Null);
        Assert.That(actualFlags, Is.EquivalentTo(expectedFlags));
    }
}
