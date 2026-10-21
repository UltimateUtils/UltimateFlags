using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class BasicFlagsSteps
{
    private readonly ILogger<BasicFlagsSteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IConfiguration _configuration;

    private readonly ServiceConfiguration _serviceConfiguration;

    public BasicFlagsSteps(
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
        _serviceConfiguration = options.Value;
    }

    [Given("DB has the following flags")]
    public async Task DbHasTheFollowingFlagsAsync(DataTable table)
    {
        IEnumerable<Flag> flags = table.CreateSet<Flag>();

        await _flagDbContext.Flags.AddRangeAsync(flags);

        _ = await _flagDbContext.SaveChangesAsync();
    }
}
