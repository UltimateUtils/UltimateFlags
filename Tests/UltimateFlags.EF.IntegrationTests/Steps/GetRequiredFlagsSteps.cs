using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using TestsBase.Utils;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Config;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class GetRequiredFlagsSteps
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

    public GetRequiredFlagsSteps(
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

    [When("I get a required flag by {string}")]
    public void GetRequiredFlagBy(string key)
    {
        try
        {
            FlagResponse actual = _flagQueryService.GetRequired(key);
            _scenarioContext.Set(actual, ContextKeys.RetrievedFlagKey);
        }
        catch (Exception e)
        {
            _scenarioContext.Set(e, ContextKeys.ThrownExceptionKey);
        }
    }

    #endregion When

    #region Then

    [Then("FlagNotFound exception is thrown")]
    public void FlagNotFoundExceptionIsThrown()
    {
        Exception x = _scenarioContext.Get<Exception>(ContextKeys.ThrownExceptionKey);

        Assert.That(x, Is.TypeOf<FlagNotFound>());
    }

    #endregion Then
}
