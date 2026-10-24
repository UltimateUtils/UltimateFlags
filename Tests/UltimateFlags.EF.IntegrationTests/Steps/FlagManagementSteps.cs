using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;
using UltimateFlags.EF.IntegrationTests.Utils;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.IntegrationTests.Steps;

[Binding]
public class FlagManagementSteps
{
    private readonly ILogger<FlagManagementSteps> _logger;

    private readonly FeatureContext _featureContext;

    private readonly ScenarioContext _scenarioContext;

    private readonly MyFlagDbContext _flagDbContext;

    private readonly IFlagService _flagService;

    private readonly IFlagQueryService _flagQueryService;

    private readonly IFlagCommandService _flagCommandService;

    private readonly IConfiguration _configuration;

    private readonly ServiceConfiguration _serviceConfiguration;

    public FlagManagementSteps(
        ILogger<FlagManagementSteps> logger,
        FeatureContext featureContext,
        ScenarioContext scenarioContext,
        MyFlagDbContext flagDbContext,
        IFlagService flagService,
        IFlagQueryService flagQueryService,
        IFlagCommandService flagCommandService,
        IConfiguration configuration,
        ServiceConfiguration serviceConfiguration)
    {
        _logger = logger;
        _featureContext = featureContext;
        _scenarioContext = scenarioContext;
        _flagDbContext = flagDbContext;
        _flagService = flagService;
        _flagQueryService = flagQueryService;
        _flagCommandService = flagCommandService;
        _configuration = configuration;
        _serviceConfiguration = serviceConfiguration;
    }

    #region Given

    [Given("The flag, {Guid}, has been deleted")]
    public void FlagHasBeenDeleted(Guid id)
    {
        _ = _flagCommandService.Delete(id);
    }

    #endregion Given

    #region When

    [When("I update a flag with the Id, {Guid}, as follows")]
    public void UpdateAFlagWithTheIdAsFollows(Guid id, DataTable table)
    {
        (bool isOn, string description) = table.CreateInstance<(bool isOn, string description)>();

        FlagResponse updateResponse = _flagCommandService.Update(id, new FlagUpdateRequest { IsOn = isOn, Description = description });

        _scenarioContext.Set(updateResponse);
    }

    [When("I delete a flag with the Id, {Guid}")]
    public void DeleteAFlagWithTheId(Guid id)
    {
        IEnumerable<FlagResponse> deletedFlags = _flagCommandService.Delete(id);
        _scenarioContext.Set(deletedFlags);
    }

    [When("I purge the flag, {Guid}")]
    public void PurgeTheFlag(Guid id)
    {
        IEnumerable<FlagResponse> purgedFlags = _flagCommandService.Purge(id);
        _scenarioContext.Set(purgedFlags);
    }

    #endregion When

    #region Then

    [Then("flag data with the Id, {Guid}, is updated properly")]
    public void FlagDataWithTheIdIsUpdatedProperly(Guid id, DataTable table)
    {
        (bool expectedIsOn, string expectedDescription) = table.CreateInstance<(bool isOn, string description)>();

        FlagResponse updateResponse = _scenarioContext.Get<FlagResponse>();

        FlagResponse? updatedFlag = _flagQueryService.Get(id);
        Assert.That(updatedFlag, Is.Not.Null);

        Assert.That(updateResponse.Id, Is.EqualTo(id));
        Assert.That(updatedFlag.Id, Is.EqualTo(id));

        Assert.That(updateResponse.IsOn, Is.EqualTo(expectedIsOn));
        Assert.That(updatedFlag.IsOn, Is.EqualTo(expectedIsOn));

        Assert.That(updateResponse.Description, Is.EqualTo(expectedDescription));
        Assert.That(updatedFlag.Description, Is.EqualTo(expectedDescription));
    }

    [Then("The flags with the following Ids have been deleted, {string}")]
    public void FlagsWithTheFollowingIdsHaveBeenDeleted(string deletedIds)
    {
        Guid[] expectedDeletedIds = [.. deletedIds.Split(',').Select(Guid.Parse)];
        IEnumerable<FlagResponse> actualDeletedFlags = _scenarioContext.Get<IEnumerable<FlagResponse>>();
        IPagedList<FlagResponse> allDeletedFlags = _flagQueryService.GetAllDeleted(1, int.MaxValue);

        Assert.That(actualDeletedFlags.Select(f => f.Id), Is.EquivalentTo(expectedDeletedIds));
        Assert.That(allDeletedFlags.Select(f => f.Id), Is.EquivalentTo(expectedDeletedIds));
    }

    [Then("The flags, {string}, are purged")]
    public void FlagsArePurged(string purgedIds)
    {
        Guid[] expectedPurgedIds = [.. purgedIds.Split(',').Select(Guid.Parse)];
    }

    #endregion Then
}
