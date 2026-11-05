using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
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

    private readonly IServiceScopeFactory _serviceScopeFactory;

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
        IServiceScopeFactory serviceScopeFactory,
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
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _serviceConfiguration = serviceConfiguration;
    }

    #region Given

    [Given("the flag, {Guid}, has been deleted")]
    public void FlagHasBeenDeleted(Guid id)
    {
        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagCommandService flagCommandService = serviceScope.ServiceProvider.GetRequiredService<IFlagCommandService>();

        IEnumerable<FlagResponse> deletedFlags = flagCommandService.Delete(id);
        _scenarioContext.Set(deletedFlags);
    }

    [Given("the flags, {string}, have been deleted")]
    public void FlagsHaveBeenDeleted(string deletedFlagIds)
    {
        Guid[] deletedIds = [.. deletedFlagIds.Split(',').Select(Guid.Parse)];

        using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        IFlagCommandService flagCommandService = serviceScope.ServiceProvider.GetRequiredService<IFlagCommandService>();

        foreach (Guid id in deletedIds)
        {
            _ = flagCommandService.Delete(id);
        }
    }

    #endregion Given

    #region When

    [When("I update a flag with the Id, {Guid}, as follows")]
    public void UpdateAFlagWithTheIdAsFollows(Guid id, DataTable table)
    {
        FlagUpdateRequest flagUpdateRequest = table.CreateInstance<FlagUpdateRequest>();

        FlagResponse updateResponse = _flagCommandService.Update(id, flagUpdateRequest);

        _scenarioContext.Set(updateResponse, ContextKeys.UpdateResponseKey);
    }

    [When("I delete a flag with the Id, {Guid}")]
    public void DeleteFlagWithTheId(Guid id)
    {
        IEnumerable<FlagResponse> deletedFlags = _flagCommandService.Delete(id);
        _scenarioContext.Set(deletedFlags);
    }

    [When("I purge the flag with the Id, {Guid}")]
    public void PurgeTheFlagWithId(Guid id)
    {
        IEnumerable<FlagResponse> purgedFlags = _flagCommandService.Purge(id);
        _scenarioContext.Set(purgedFlags);
    }

    [When("I restore the flag, {Guid}")]
    public void RestoreFlag(Guid idToRestore)
    {
        IEnumerable<FlagResponse> restoredFlags = _flagCommandService.Restore(idToRestore);
        _scenarioContext.Set(restoredFlags);
    }

    #endregion When

    #region Then

    [Then("flag data with the Id, {Guid}, is updated properly")]
    public void FlagDataWithTheIdIsUpdatedProperly(Guid id, DataTable table)
    {
        Flag expectedFlag = table.CreateInstance<Flag>();

        FlagResponse updateResponse = _scenarioContext.Get<FlagResponse>(ContextKeys.UpdateResponseKey);

        FlagResponse? updatedFlag = _flagQueryService.Get(id);
        Assert.That(updatedFlag, Is.Not.Null);

        Assert.That(updateResponse.Id, Is.EqualTo(id));
        Assert.That(updatedFlag.Id, Is.EqualTo(id));

        Assert.That(updateResponse.IsOn, Is.EqualTo(expectedFlag.IsOn));
        Assert.That(updatedFlag.IsOn, Is.EqualTo(expectedFlag.IsOn));

        Assert.That(updateResponse.Description, Is.EqualTo(expectedFlag.Description));
        Assert.That(updatedFlag.Description, Is.EqualTo(expectedFlag.Description));
    }

    [Then("exactly the flags with the following Ids are deleted, {string}")]
    public void ExactlyFlagsWithTheFollowingIdsAreDeleted(string deletedIds)
    {
        Guid[] expectedDeletedIds = [.. deletedIds.Split(',').Select(Guid.Parse)];
        IEnumerable<FlagResponse> actualDeletedFlags = _scenarioContext.Get<IEnumerable<FlagResponse>>();
        IPagedList<FlagResponse> allDeletedFlags = _flagQueryService.GetAllDeleted(1, int.MaxValue);

        Assert.That(actualDeletedFlags.Select(f => f.Id), Is.EquivalentTo(expectedDeletedIds));
        Assert.That(allDeletedFlags.Select(f => f.Id), Is.EquivalentTo(expectedDeletedIds));
    }

    [Then("the flags with the following Ids are not deleted, {string}")]
    public void FlagsWithFollowingIdsAreNotDeleted(string notDeletedIds)
    {
        Guid[] expectedNotDeletedIds = [.. notDeletedIds.Split(',').Select(Guid.Parse)];

        IPagedList<FlagResponse> allDeletedFlags = _flagQueryService.GetAllDeleted(1, int.MaxValue);

        Assert.That(allDeletedFlags.Select(f => f.Id).Intersect(expectedNotDeletedIds), Is.Empty);
    }

    [Then("the flags, {string}, are purged")]
    public void FlagsArePurged(string purgedIds)
    {
        Guid[] expectedPurgedIds = [.. purgedIds.Split(',').Select(Guid.Parse)];

        foreach (Guid expectedPurgedId in expectedPurgedIds)
        {
            FlagResponse? foundFlag = _flagQueryService.Get(expectedPurgedId);
            Assert.That(foundFlag, Is.Null);
        }

        FlagResponse[] allDeleted = [.. _flagQueryService.GetAllDeleted(1, int.MaxValue)];
        Assert.That(allDeleted.Select(f => f.Id).Intersect(expectedPurgedIds), Is.Empty);
    }

    [Then("no flags left soft-deleted")]
    public void NoFlagsLeftSoftDeleted()
    {
        IPagedList<FlagResponse> deletedFlags = _flagQueryService.GetAllDeleted();

        Assert.That(deletedFlags, Is.Empty);
    }

    [Then("the flags, {string}, left soft-deleted")]
    public void FlagsLeftSoftDeleted(string deletedIdsString)
    {
        Guid[] expectedDeletedIds = [.. deletedIdsString.Split(',').Select(Guid.Parse)];

        IPagedList<FlagResponse> deletedFlags = _flagQueryService.GetAllDeleted(1, int.MaxValue);

        Assert.That(expectedDeletedIds, Is.SubsetOf(deletedFlags.Select(f => f.Id)));
    }

    [Then("the response has the flags, {string}")]
    public void ResponseHasFlags(string restoredIds)
    {
        IEnumerable<FlagResponse> restoredFlags = _scenarioContext.Get<IEnumerable<FlagResponse>>();

        Guid[] expectedRestoredIds = [.. restoredIds.Split(',').Select(Guid.Parse)];

        Assert.That(restoredFlags.Select(f => f.Id), Is.EquivalentTo(expectedRestoredIds));
    }

    [Then("the flags, {string}, are restored")]
    public void FlagsAreRestored(string restoredIds)
    {
        Guid[] expectedRestoredIds = [.. restoredIds.Split(',').Select(Guid.Parse)];

        IPagedList<FlagResponse> deletedFlags = _flagQueryService.GetAllDeleted(1, int.MaxValue);

        IEnumerable<Guid> deletedFlagIds = deletedFlags.Select(f => f.Id);

        Assert.That(expectedRestoredIds.Intersect(deletedFlagIds), Is.Empty);
    }

    [Then("no exception is thrown")]
    public void NoExceptionIsThrown()
    {
        Exception x = _scenarioContext.Get<Exception>(ContextKeys.ThrownExceptionKey);

        Assert.That(x, Is.TypeOf<NoException>());
    }

    #endregion Then
}
