using Reqnroll;

namespace UltimateFlags.Configuration.IntegrationTests.Steps;

[Binding]
public class GetFlagsSteps
{
    #region When

    [When("retrieve a flag by {Guid}")]
    public void RetrieveFlagById(Guid id)
    {
        throw new PendingStepException();
    }

    [When("I retrieve a flag by {string} and {Guid}")]
    public void RetrieveFlagByAndParentId(string myBuilding, Guid? parentId)
    {
        throw new PendingStepException();
    }

    [When("I retrieve a flag by {string}")]
    public void RetrieveFlagByKey(string key)
    {
        throw new PendingStepException();
    }

    [When("I retrieve all deleted flags")]
    public void RetrieveAllDeletedFlags()
    {
        throw new PendingStepException();
    }

    #endregion When

    #region Then

    [Then("the following flag is returned correctly")]
    public void FollowingFlagIsReturnedCorrectly(DataTable table)
    {
        throw new PendingStepException();
    }

    [Then("null is returned")]
    public void NullIsReturned()
    {
        throw new PendingStepException();
    }

    [Then("the following flags are returned")]
    public void FlagsAreReturned(IEnumerable<Guid?> ids)
    {
        var x = ids.ToList();
        throw new PendingStepException();
    }

    #endregion Then
}
