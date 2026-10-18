using Reqnroll;
using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class BasicGetFlagsSteps
{
    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    public BasicGetFlagsSteps(
        FeatureContext featureContext,
        ScenarioContext scenarioContext)
    {
        _featureContext = featureContext;
        _scenarioContext = scenarioContext;
    }

    [Given("DB has the following flags")]
    public void DbHasTheFollowingFlags(DataTable table)
    {
        IEnumerable<Flag> flags = table.CreateSet<Flag>();

        throw new PendingStepException();
    }

    [When("retrieve all flags")]
    public void RetrieveAllFlags()
    {
        throw new PendingStepException();
    }

    [Then("all flags are returned correctly")]
    public void AllFlagsAreReturnedCorrectly()
    {
        throw new PendingStepException();
    }
}
