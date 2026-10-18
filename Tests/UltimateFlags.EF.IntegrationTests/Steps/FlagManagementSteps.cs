using Reqnroll;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class FlagManagementSteps
{
    private readonly ScenarioContext _scenarioContext;

    public FlagManagementSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("DB is ready")]
    public void DbIsReady()
    {
        throw new PendingStepException();
    }

    [When("flag is added")]
    public void FlagIsAdded()
    {
        throw new PendingStepException();
    }

    [Then("flag data is populated properly")]
    public void FlagDataIsPopulatedProperly()
    {
        throw new PendingStepException();
    }
}
