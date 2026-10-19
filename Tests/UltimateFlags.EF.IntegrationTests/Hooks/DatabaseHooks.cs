using Reqnroll;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Hooks;

[Binding]
public class DatabaseHooks
{
    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    public DatabaseHooks(
        ScenarioContext scenarioContext,
        MyFlagDbContext flagDbContext)
    {
        _scenarioContext = scenarioContext;
        _flagDbContext = flagDbContext;
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
