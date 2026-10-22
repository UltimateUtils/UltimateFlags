using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reqnroll;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;

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

    [BeforeFeature("ResetBasicFlags")]
    [BeforeScenario("ResetBasicFlags")]
    public static async Task ResetBasicFlagsAsync(MyFlagDbContext flagDbContext)
    {
        _ = await flagDbContext.Flags.ExecuteDeleteAsync();

        IEnumerable<Flag> flags = _GetBasicFlags();

        await flagDbContext.Flags.AddRangeAsync(flags);
        _ = await flagDbContext.SaveChangesAsync();
    }


    [AfterFeature]
    public static async Task CleanupTablesAsync(MyFlagDbContext flagDbContext)
    {
        _ = await flagDbContext.Flags.ExecuteDeleteAsync();
    }

    //// This method will generate the BASIC FLAGS
    //// | Id                                   | Name       | Key                                    | IsOn  | Description   | ParentId                             | CreatedAt                   | UpdatedAt                   | DeletedAt |
    //// | 4F228BF5-BBB0-428C-92CE-B63754579712 | MyBuilding | MyBuilding                             | TRUE  | my building   | <null>                               | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>    |
    //// | CDFA551C-5504-4475-97BE-66A9084C81A7 | Floor23    | MyBuilding.Floor23                     | TRUE  | floor #23     | 4F228BF5-BBB0-428C-92CE-B63754579712 | 2026-09-30 03:38:26.5139868 | 2026-09-30 03:38:26.5139868 | <null>    |
    //// | 12A46EC8-AFFB-4E6A-BABF-D1F92E661FE5 | Room2301   | MyBuilding.Floor23.Room2301            | FALSE | room #2301    | CDFA551C-5504-4475-97BE-66A9084C81A7 | 2026-09-30 03:38:50.2655434 | 2026-09-30 03:38:50.2655434 | <null>    |
    //// | 6F3BD2C8-F17C-4903-B3BE-C1CA88EABCCE | Room2302   | MyBuilding.Floor23.Room2302            | TRUE  | room #2302    | CDFA551C-5504-4475-97BE-66A9084C81A7 | 2026-09-30 03:38:58.7826112 | 2026-09-30 03:38:58.7826112 | <null>    |
    //// | 6CA466A8-11E3-4751-B69A-D06714CCEEA1 | Room2303   | MyBuilding.Floor23.Room2303            | FALSE | room #2303    | CDFA551C-5504-4475-97BE-66A9084C81A7 | 2026-09-30 03:39:01.7470543 | 2026-09-30 03:39:01.7470543 | <null>    |
    //// | 705FA822-E54B-4505-87B5-5EDC1E69D780 | Room2304   | MyBuilding.Floor23.Room2304            | TRUE  | room #2304    | CDFA551C-5504-4475-97BE-66A9084C81A7 | 2026-09-30 03:39:04.4400251 | 2026-09-30 03:39:04.4400251 | <null>    |
    //// | 404FA049-CBB7-4C6E-A143-3340678CA90E | Floor24    | MyBuilding.Floor24                     | FALSE | floor #24     | 4F228BF5-BBB0-428C-92CE-B63754579712 | 2026-09-30 03:41:02.8863859 | 2026-09-30 03:41:02.8863859 | <null>    |
    //// | 58E492D1-5723-427A-883F-5F5F6B661210 | Room2401   | MyBuilding.Floor24.Room2401            | TRUE  | Room #2401    | 404FA049-CBB7-4C6E-A143-3340678CA90E | 2026-09-30 03:41:32.8614085 | 2026-09-30 03:41:32.8614085 | <null>    |
    //// | 65BDBC19-9125-426C-B792-6EB8E272D5A2 | Table2401a | MyBuilding.Floor24.Room2401.Table2401a | TRUE  | table #2401 a | 58E492D1-5723-427A-883F-5F5F6B661210 | 2026-09-30 03:42:10.6716511 | 2026-09-30 03:42:10.6716511 | <null>    |
    //// | 04485343-B041-4603-82FF-D08B13292127 | Table2401b | MyBuilding.Floor24.Room2401.Table2401b | TRUE  | table #2401 b | 58E492D1-5723-427A-883F-5F5F6B661210 | 2026-09-30 03:42:13.7318341 | 2026-09-30 03:42:13.7318341 | <null>    |
    //// | 9978B252-97BE-429C-B7A8-63F6579373E6 | Table2401c | MyBuilding.Floor24.Room2401.Table2401c | FALSE | table #2401 c | 58E492D1-5723-427A-883F-5F5F6B661210 | 2026-09-30 03:42:16.7083398 | 2026-09-30 03:42:16.7083398 | <null>    |
    //// | DCCA661B-98A2-4FDF-8219-8B7A340A3688 | Room2402   | MyBuilding.Floor24.Room2402            | TRUE  | room #2402    | 404FA049-CBB7-4C6E-A143-3340678CA90E | 2026-09-30 03:43:00.2007052 | 2026-09-30 03:43:00.2007052 | <null>    |
    //// | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | Room2403   | MyBuilding.Floor24.Room2403            | TRUE  | room #2403    | 404FA049-CBB7-4C6E-A143-3340678CA90E | 2026-09-30 03:43:19.7272716 | 2026-09-30 03:43:19.7272716 | <null>    |
    //// | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | Table2403a | MyBuilding.Floor24.Room2403.Table2403a | TRUE  | table #2403 a | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | 2026-09-30 03:43:49.1854194 | 2026-09-30 03:43:49.1854194 | <null>    |
    //// | 09A4D20D-B63F-4120-BC95-9A0A6532ADD1 | Table2403b | MyBuilding.Floor24.Room2403.Table2403b | TRUE  | table #2403 b | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | 2026-09-30 03:43:52.2221069 | 2026-09-30 03:43:52.2221069 | <null>    |
    //// | 56CF92D4-0FFF-40D5-B7B4-E78E03961650 | Room2404   | MyBuilding.Floor24.Room2404            | TRUE  | room #2404    | 404FA049-CBB7-4C6E-A143-3340678CA90E | 2026-09-30 03:44:24.5148005 | 2026-09-30 03:44:24.5148005 | <null>    |
    //// | 06DD9F21-626B-4B88-93F7-BE8B2E4FC3F0 | Room2405   | MyBuilding.Floor24.Room2405            | TRUE  | room #2405    | 404FA049-CBB7-4C6E-A143-3340678CA90E | 2026-09-30 03:44:33.9484467 | 2026-09-30 03:44:33.9484467 | <null>    |
    //// | 52B40BAA-183B-4C6E-A8E9-71AF2F5F797F | Floor25    | MyBuilding.Floor25                     | TRUE  | floor #25     | 4F228BF5-BBB0-428C-92CE-B63754579712 | 2026-09-30 03:45:23.2836617 | 2026-09-30 03:45:23.2836617 | <null>    |
    //// | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | Floor26    | MyBuilding.Floor26                     | TRUE  | floor #26     | 4F228BF5-BBB0-428C-92CE-B63754579712 | 2026-09-30 03:45:45.9977765 | 2026-09-30 03:45:45.9977765 | <null>    |
    //// | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | Room2601   | MyBuilding.Floor26.Room2601            | FALSE | room #2601    | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | 2026-09-30 03:46:10.4785162 | 2026-09-30 03:46:10.4785162 | <null>    |
    //// | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | Room2602   | MyBuilding.Floor26.Room2602            | TRUE  | room #2602    | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | 2026-09-30 03:46:17.4000942 | 2026-09-30 03:46:17.4000942 | <null>    |
    //// | 9B503318-BDB1-4EA7-8D2D-306C1E974E70 | Table2602a | MyBuilding.Floor26.Room2602.Table2602a | TRUE  | table #2602 a | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | 2026-09-30 03:46:38.2760252 | 2026-09-30 03:46:38.2760252 | <null>    |
    //// | 1430860F-FEA7-4318-BE0D-5B78157FA328 | Table2602b | MyBuilding.Floor26.Room2602.Table2602b | TRUE  | table #2602 b | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | 2026-09-30 03:46:44.7748539 | 2026-09-30 03:46:44.7748539 | <null>    |
    //// | DBE0F6F1-9338-497A-998C-234AD831CFB6 | Table2602c | MyBuilding.Floor26.Room2602.Table2602c | FALSE | table #2602 c | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | 2026-10-17 20:36:30.3876071 | 2026-10-17 20:36:30.3876071 | <null>    |
    //// | 58934C81-05C6-4E0B-A62D-24996957D486 | Table2602d | MyBuilding.Floor26.Room2602.Table2602d | TRUE  | table #2602 d | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | 2026-10-17T20:36:30.3876072 | 2026-10-17T20:36:30.3876072 | <null>    |
    private static IEnumerable<Flag> _GetBasicFlags()
    {
        yield return
            new Flag
            {
                Id = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                Name = "MyBuilding",
                Key = "MyBuilding",
                IsOn = true,
                Description = "my building",
                ParentId = null,
                CreatedAt = DateTime.Parse("2026-09-30 03:37:26.0661723"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:37:26.0661723"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                Name = "Floor23",
                Key = "MyBuilding.Floor23",
                IsOn = true,
                Description = "floor #23",
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                CreatedAt = DateTime.Parse("2026-09-30 03:38:26.5139868"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:38:26.5139868"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("12A46EC8-AFFB-4E6A-BABF-D1F92E661FE5"),
                Name = "Room2301",
                Key = "MyBuilding.Floor23.Room2301",
                IsOn = false,
                Description = "room #2301",
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                CreatedAt = DateTime.Parse("2026-09-30 03:38:50.2655434"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:38:50.2655434"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("6F3BD2C8-F17C-4903-B3BE-C1CA88EABCCE"),
                Name = "Room2302",
                Key = "MyBuilding.Floor23.Room2302",
                IsOn = true,
                Description = "room #2302",
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                CreatedAt = DateTime.Parse("2026-09-30 03:38:58.7826112"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:38:58.7826112"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("6CA466A8-11E3-4751-B69A-D06714CCEEA1"),
                Name = "Room2303",
                Key = "MyBuilding.Floor23.Room2303",
                IsOn = false,
                Description = "room #2303",
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                CreatedAt = DateTime.Parse("2026-09-30 03:39:01.7470543"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:39:01.7470543"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("705FA822-E54B-4505-87B5-5EDC1E69D780"),
                Name = "Room2304",
                Key = "MyBuilding.Floor23.Room2304",
                IsOn = true,
                Description = "room #2304",
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                CreatedAt = DateTime.Parse("2026-09-30 03:39:04.4400251"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:39:04.4400251"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                Name = "Floor24",
                Key = "MyBuilding.Floor24",
                IsOn = false,
                Description = "floor #24",
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                CreatedAt = DateTime.Parse("2026-09-30 03:41:02.8863859"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:41:02.8863859"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                Name = "Room2401",
                Key = "MyBuilding.Floor24.Room2401",
                IsOn = true,
                Description = "Room #2401",
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                CreatedAt = DateTime.Parse("2026-09-30 03:41:32.8614085"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:41:32.8614085"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("65BDBC19-9125-426C-B792-6EB8E272D5A2"),
                Name = "Table2401a",
                Key = "MyBuilding.Floor24.Room2401.Table2401a",
                IsOn = true,
                Description = "table #2401 a",
                ParentId = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                CreatedAt = DateTime.Parse("2026-09-30 03:42:10.6716511"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:42:10.6716511"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("04485343-B041-4603-82FF-D08B13292127"),
                Name = "Table2401b",
                Key = "MyBuilding.Floor24.Room2401.Table2401b",
                IsOn = true,
                Description = "table #2401 b",
                ParentId = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                CreatedAt = DateTime.Parse("2026-09-30 03:42:13.7318341"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:42:13.7318341"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("9978B252-97BE-429C-B7A8-63F6579373E6"),
                Name = "Table2401c",
                Key = "MyBuilding.Floor24.Room2401.Table2401c",
                IsOn = false,
                Description = "table #2401 c",
                ParentId = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                CreatedAt = DateTime.Parse("2026-09-30 03:42:16.7083398"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:42:16.7083398"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("DCCA661B-98A2-4FDF-8219-8B7A340A3688"),
                Name = "Room2402",
                Key = "MyBuilding.Floor24.Room2402",
                IsOn = true,
                Description = "room #2402",
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:00.2007052"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:00.2007052"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD"),
                Name = "Room2403",
                Key = "MyBuilding.Floor24.Room2403",
                IsOn = true,
                Description = "room #2403",
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:19.7272716"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:19.7272716"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("A63D23CC-3A85-4B59-A420-CBFD1E127FC5"),
                Name = "Table2403a",
                Key = "MyBuilding.Floor24.Room2403.Table2403a",
                IsOn = true,
                Description = "table #2403 a",
                ParentId = Guid.Parse("F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:49.1854194"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:49.1854194"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("09A4D20D-B63F-4120-BC95-9A0A6532ADD1"),
                Name = "Table2403b",
                Key = "MyBuilding.Floor24.Room2403.Table2403b",
                IsOn = true,
                Description = "table #2403 b",
                ParentId = Guid.Parse("F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:52.2221069"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:52.2221069"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("56CF92D4-0FFF-40D5-B7B4-E78E03961650"),
                Name = "Room2404",
                Key = "MyBuilding.Floor24.Room2404",
                IsOn = true,
                Description = "room #2404",
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                CreatedAt = DateTime.Parse("2026-09-30 03:44:24.5148005"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:44:24.5148005"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("06DD9F21-626B-4B88-93F7-BE8B2E4FC3F0"),
                Name = "Room2405",
                Key = "MyBuilding.Floor24.Room2405",
                IsOn = true,
                Description = "room #2405",
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                CreatedAt = DateTime.Parse("2026-09-30 03:44:33.9484467"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:44:33.9484467"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("52B40BAA-183B-4C6E-A8E9-71AF2F5F797F"),
                Name = "Floor25",
                Key = "MyBuilding.Floor25",
                IsOn = true,
                Description = "floor #25",
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                CreatedAt = DateTime.Parse("2026-09-30 03:45:23.2836617"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:45:23.2836617"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6"),
                Name = "Floor26",
                Key = "MyBuilding.Floor26",
                IsOn = true,
                Description = "floor #26",
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                CreatedAt = DateTime.Parse("2026-09-30 03:45:45.9977765"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:45:45.9977765"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("1AEE1C30-B4BA-4B5D-9BBE-0078C372579F"),
                Name = "Room2601",
                Key = "MyBuilding.Floor26.Room2601",
                IsOn = false,
                Description = "room #2601",
                ParentId = Guid.Parse("10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:10.4785162"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:10.4785162"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                Name = "Room2602",
                Key = "MyBuilding.Floor26.Room2602",
                IsOn = true,
                Description = "room #2602",
                ParentId = Guid.Parse("10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:17.4000942"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:17.4000942"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("9B503318-BDB1-4EA7-8D2D-306C1E974E70"),
                Name = "Table2602a",
                Key = "MyBuilding.Floor26.Room2602.Table2602a",
                IsOn = true,
                Description = "table #2602 a",
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:38.2760252"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:38.2760252"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("1430860F-FEA7-4318-BE0D-5B78157FA328"),
                Name = "Table2602b",
                Key = "MyBuilding.Floor26.Room2602.Table2602b",
                IsOn = true,
                Description = "table #2602 b",
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:44.7748539"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:44.7748539"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("DBE0F6F1-9338-497A-998C-234AD831CFB6"),
                Name = "Table2602c",
                Key = "MyBuilding.Floor26.Room2602.Table2602c",
                IsOn = false,
                Description = "table #2602 c",
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                CreatedAt = DateTime.Parse("2026-10-17 20:36:30.3876071"),
                UpdatedAt = DateTime.Parse("2026-10-17 20:36:30.3876071"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("58934C81-05C6-4E0B-A62D-24996957D486"),
                Name = "Table2602d",
                Key = "MyBuilding.Floor26.Room2602.Table2602d",
                IsOn = true,
                Description = "table #2602 d",
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                CreatedAt = DateTime.Parse("2026-10-17T20:36:30.3876072"),
                UpdatedAt = DateTime.Parse("2026-10-17T20:36:30.3876072"),
                DeletedAt = null,
            };
    }
}
