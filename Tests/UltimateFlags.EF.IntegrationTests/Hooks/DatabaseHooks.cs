using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;
using UltimateFlags.EF.IntegrationTests.Steps;
using UltimateFlags.EF.IntegrationTests.Utils;

namespace UltimateFlags.EF.IntegrationTests.Hooks;

[Binding]
public class DatabaseHooks
{
    private readonly ILogger<DatabaseHooks> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IConfiguration _configuration;

    private readonly IOptions<ServiceConfiguration> _options;

    public DatabaseHooks(
        ILogger<DatabaseHooks> logger,
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

    [BeforeScenario]
    public void DoSomethingBeforeScenario()
    {
    }

    [AfterScenario]
    public void DoSomethingAfterScenario()
    {
    }
}
