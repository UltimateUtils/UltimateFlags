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
        await _CleanupFlagTableAsync(flagDbContext);

        IEnumerable<Flag> flags = _GetBasicFlags();

        await flagDbContext.Flags.AddRangeAsync(flags);
        _ = await flagDbContext.SaveChangesAsync();
    }

    [AfterFeature]
    public static async Task CleanupTablesAsync(MyFlagDbContext flagDbContext)
    {
        await _CleanupFlagTableAsync(flagDbContext);
    }

    private static async Task _CleanupFlagTableAsync(MyFlagDbContext flagDbContext)
    {
        _ =
            await
                flagDbContext
                    .Flags
                    .IgnoreQueryFilters()
                    .ExecuteUpdateAsync(s => s.SetProperty(e => e.ParentId, (Guid?)null));

        _ =
            await
                flagDbContext
                    .Flags
                    .IgnoreQueryFilters()
                    .ExecuteDeleteAsync();
    }

    //// This method will generate the BASIC FLAGS
    ////
    ////   | Id                                   | ParentId                             | IsOn | Name       | Key                                    | Description   | ConcurrencyToken                     | CreatedAt                   | UpdatedAt                   | DeletedAt |
    ////   | 4F228BF5-BBB0-428C-92CE-B63754579712 | <null>                               | TRUE | MyBuilding | MyBuilding                             | my building   | C04D5590-AC01-48D2-957E-2F3D37E3F70F | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>    |
    ////   | CDFA551C-5504-4475-97BE-66A9084C81A7 | 4F228BF5-BBB0-428C-92CE-B63754579712 | TRUE | Floor23    | MyBuilding.Floor23                     | floor #23     | 1EE72A0B-16AF-428D-908F-2ADB5214E1D4 | 2026-09-30 03:38:26.5139868 | 2026-09-30 03:38:26.5139868 | <null>    |
    ////   | 12A46EC8-AFFB-4E6A-BABF-D1F92E661FE5 | CDFA551C-5504-4475-97BE-66A9084C81A7 | TRUE | Room2301   | MyBuilding.Floor23.Room2301            | room #2301    | D7601C7A-EE41-4ED6-B48A-E3DDB7682796 | 2026-09-30 03:38:50.2655434 | 2026-09-30 03:38:50.2655434 | <null>    |
    ////   | 6F3BD2C8-F17C-4903-B3BE-C1CA88EABCCE | CDFA551C-5504-4475-97BE-66A9084C81A7 | TRUE | Room2302   | MyBuilding.Floor23.Room2302            | room #2302    | C530934B-B93C-43F0-9480-F00142570BF4 | 2026-09-30 03:38:58.7826112 | 2026-09-30 03:38:58.7826112 | <null>    |
    ////   | 6CA466A8-11E3-4751-B69A-D06714CCEEA1 | CDFA551C-5504-4475-97BE-66A9084C81A7 | TRUE | Room2303   | MyBuilding.Floor23.Room2303            | room #2303    | 1167C619-C3E4-47CA-9032-D4BC2203FB3C | 2026-09-30 03:39:01.7470543 | 2026-09-30 03:39:01.7470543 | <null>    |
    ////   | 705FA822-E54B-4505-87B5-5EDC1E69D780 | CDFA551C-5504-4475-97BE-66A9084C81A7 | TRUE | Room2304   | MyBuilding.Floor23.Room2304            | room #2304    | 8F733BC5-677A-4F6B-AE35-C035C2744CA1 | 2026-09-30 03:39:04.4400251 | 2026-09-30 03:39:04.4400251 | <null>    |
    ////   | 404FA049-CBB7-4C6E-A143-3340678CA90E | 4F228BF5-BBB0-428C-92CE-B63754579712 | TRUE | Floor24    | MyBuilding.Floor24                     | floor #24     | CBECE282-0B5F-43C7-A541-E65F8EDAF054 | 2026-09-30 03:41:02.8863859 | 2026-09-30 03:41:02.8863859 | <null>    |
    ////   | 58E492D1-5723-427A-883F-5F5F6B661210 | 404FA049-CBB7-4C6E-A143-3340678CA90E | TRUE | Room2401   | MyBuilding.Floor24.Room2401            | Room #2401    | 390AF1AF-5D53-48F1-B943-DDFBCB43C82B | 2026-09-30 03:41:32.8614085 | 2026-09-30 03:41:32.8614085 | <null>    |
    ////   | 65BDBC19-9125-426C-B792-6EB8E272D5A2 | 58E492D1-5723-427A-883F-5F5F6B661210 | TRUE | Table2401a | MyBuilding.Floor24.Room2401.Table2401a | table #2401 a | 2FA325D2-792D-4B6C-9615-6DD9C8BCF95C | 2026-09-30 03:42:10.6716511 | 2026-09-30 03:42:10.6716511 | <null>    |
    ////   | 04485343-B041-4603-82FF-D08B13292127 | 58E492D1-5723-427A-883F-5F5F6B661210 | TRUE | Table2401b | MyBuilding.Floor24.Room2401.Table2401b | table #2401 b | 411611E3-E849-4B78-8092-AD8FFFAE3C6C | 2026-09-30 03:42:13.7318341 | 2026-09-30 03:42:13.7318341 | <null>    |
    ////   | 9978B252-97BE-429C-B7A8-63F6579373E6 | 58E492D1-5723-427A-883F-5F5F6B661210 | TRUE | Table2401c | MyBuilding.Floor24.Room2401.Table2401c | table #2401 c | 84A515F0-6FAC-4EA3-A799-898F1D006CD5 | 2026-09-30 03:42:16.7083398 | 2026-09-30 03:42:16.7083398 | <null>    |
    ////   | DCCA661B-98A2-4FDF-8219-8B7A340A3688 | 404FA049-CBB7-4C6E-A143-3340678CA90E | TRUE | Room2402   | MyBuilding.Floor24.Room2402            | room #2402    | D1B85423-A8AB-46BE-B726-7D6F5875AE86 | 2026-09-30 03:43:00.2007052 | 2026-09-30 03:43:00.2007052 | <null>    |
    ////   | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | 404FA049-CBB7-4C6E-A143-3340678CA90E | TRUE | Room2403   | MyBuilding.Floor24.Room2403            | room #2403    | 985E8726-E0BF-4BC8-8086-E99B101D00A5 | 2026-09-30 03:43:19.7272716 | 2026-09-30 03:43:19.7272716 | <null>    |
    ////   | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | TRUE | Table2403a | MyBuilding.Floor24.Room2403.Table2403a | table #2403 a | 7A8C9CDA-E5C3-4B89-B708-FFACBD29A3BD | 2026-09-30 03:43:49.1854194 | 2026-09-30 03:43:49.1854194 | <null>    |
    ////   | 09A4D20D-B63F-4120-BC95-9A0A6532ADD1 | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | TRUE | Table2403b | MyBuilding.Floor24.Room2403.Table2403b | table #2403 b | A3D1C2E4-A681-463F-AF49-131289720675 | 2026-09-30 03:43:52.2221069 | 2026-09-30 03:43:52.2221069 | <null>    |
    ////   | 56CF92D4-0FFF-40D5-B7B4-E78E03961650 | 404FA049-CBB7-4C6E-A143-3340678CA90E | TRUE | Room2404   | MyBuilding.Floor24.Room2404            | room #2404    | AE25278C-7AD1-48E8-BDBC-7DC5529C5899 | 2026-09-30 03:44:24.5148005 | 2026-09-30 03:44:24.5148005 | <null>    |
    ////   | 06DD9F21-626B-4B88-93F7-BE8B2E4FC3F0 | 404FA049-CBB7-4C6E-A143-3340678CA90E | TRUE | Room2405   | MyBuilding.Floor24.Room2405            | room #2405    | 720EA77C-52ED-4815-B1CF-AC06C5705E3C | 2026-09-30 03:44:33.9484467 | 2026-09-30 03:44:33.9484467 | <null>    |
    ////   | 52B40BAA-183B-4C6E-A8E9-71AF2F5F797F | 4F228BF5-BBB0-428C-92CE-B63754579712 | TRUE | Floor25    | MyBuilding.Floor25                     | floor #25     | D066581F-4566-41EC-8CEE-75AB2519E26C | 2026-09-30 03:45:23.2836617 | 2026-09-30 03:45:23.2836617 | <null>    |
    ////   | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | 4F228BF5-BBB0-428C-92CE-B63754579712 | TRUE | Floor26    | MyBuilding.Floor26                     | floor #26     | 06C2F42C-6BC6-4DD8-A81F-279F4F194F92 | 2026-09-30 03:45:45.9977765 | 2026-09-30 03:45:45.9977765 | <null>    |
    ////   | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | TRUE | Room2601   | MyBuilding.Floor26.Room2601            | room #2601    | 77E5F5EC-43AB-4EA8-8808-449FA24B57B1 | 2026-09-30 03:46:10.4785162 | 2026-09-30 03:46:10.4785162 | <null>    |
    ////   | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | TRUE | Room2602   | MyBuilding.Floor26.Room2602            | room #2602    | 76B48458-58BB-4445-97C2-0FE10F94C168 | 2026-09-30 03:46:17.4000942 | 2026-09-30 03:46:17.4000942 | <null>    |
    ////   | 9B503318-BDB1-4EA7-8D2D-306C1E974E70 | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | TRUE | Table2602a | MyBuilding.Floor26.Room2602.Table2602a | table #2602 a | 741BA645-DC05-490A-97C7-F403EA559DC4 | 2026-09-30 03:46:38.2760252 | 2026-09-30 03:46:38.2760252 | <null>    |
    ////   | 1430860F-FEA7-4318-BE0D-5B78157FA328 | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | TRUE | Table2602b | MyBuilding.Floor26.Room2602.Table2602b | table #2602 b | FB0CF018-D4AA-4652-BFCB-1790EC9F7B0B | 2026-09-30 03:46:44.7748539 | 2026-09-30 03:46:44.7748539 | <null>    |
    ////   | DBE0F6F1-9338-497A-998C-234AD831CFB6 | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | TRUE | Table2602c | MyBuilding.Floor26.Room2602.Table2602c | table #2602 c | 3EC5281B-EE4D-410B-B477-BD8D4CE9FD81 | 2026-10-17 20:36:30.3876071 | 2026-10-17 20:36:30.3876071 | <null>    |
    ////   | 58934C81-05C6-4E0B-A62D-24996957D486 | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | TRUE | Table2602d | MyBuilding.Floor26.Room2602.Table2602d | table #2602 d | 4F8F0842-4B70-4C3F-9B29-8DBDAC978268 | 2026-10-17 20:36:30.3876072 | 2026-10-17 20:36:30.3876072 | <null>    |
    private static IEnumerable<Flag> _GetBasicFlags()
    {
        yield return
            new Flag
            {
                Id = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                ParentId = null,
                IsOn = true,
                Name = "MyBuilding",
                Key = "MyBuilding",
                Description = "my building",
                ConcurrencyToken = Guid.Parse("C04D5590-AC01-48D2-957E-2F3D37E3F70F"),
                CreatedAt = DateTime.Parse("2026-09-30 03:37:26.0661723"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:37:26.0661723"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                IsOn = true,
                Name = "Floor23",
                Key = "MyBuilding.Floor23",
                Description = "floor #23",
                ConcurrencyToken = Guid.Parse("1EE72A0B-16AF-428D-908F-2ADB5214E1D4"),
                CreatedAt = DateTime.Parse("2026-09-30 03:38:26.5139868"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:38:26.5139868"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("12A46EC8-AFFB-4E6A-BABF-D1F92E661FE5"),
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                IsOn = true,
                Name = "Room2301",
                Key = "MyBuilding.Floor23.Room2301",
                Description = "room #2301",
                ConcurrencyToken = Guid.Parse("D7601C7A-EE41-4ED6-B48A-E3DDB7682796"),
                CreatedAt = DateTime.Parse("2026-09-30 03:38:50.2655434"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:38:50.2655434"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("6F3BD2C8-F17C-4903-B3BE-C1CA88EABCCE"),
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                IsOn = true,
                Name = "Room2302",
                Key = "MyBuilding.Floor23.Room2302",
                Description = "room #2302",
                ConcurrencyToken = Guid.Parse("C530934B-B93C-43F0-9480-F00142570BF4"),
                CreatedAt = DateTime.Parse("2026-09-30 03:38:58.7826112"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:38:58.7826112"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("6CA466A8-11E3-4751-B69A-D06714CCEEA1"),
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                IsOn = true,
                Name = "Room2303",
                Key = "MyBuilding.Floor23.Room2303",
                Description = "room #2303",
                ConcurrencyToken = Guid.Parse("1167C619-C3E4-47CA-9032-D4BC2203FB3C"),
                CreatedAt = DateTime.Parse("2026-09-30 03:39:01.7470543"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:39:01.7470543"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("705FA822-E54B-4505-87B5-5EDC1E69D780"),
                ParentId = Guid.Parse("CDFA551C-5504-4475-97BE-66A9084C81A7"),
                IsOn = true,
                Name = "Room2304",
                Key = "MyBuilding.Floor23.Room2304",
                Description = "room #2304",
                ConcurrencyToken = Guid.Parse("8F733BC5-677A-4F6B-AE35-C035C2744CA1"),
                CreatedAt = DateTime.Parse("2026-09-30 03:39:04.4400251"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:39:04.4400251"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                IsOn = true,
                Name = "Floor24",
                Key = "MyBuilding.Floor24",
                Description = "floor #24",
                ConcurrencyToken = Guid.Parse("CBECE282-0B5F-43C7-A541-E65F8EDAF054"),
                CreatedAt = DateTime.Parse("2026-09-30 03:41:02.8863859"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:41:02.8863859"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                IsOn = true,
                Name = "Room2401",
                Key = "MyBuilding.Floor24.Room2401",
                Description = "Room #2401",
                ConcurrencyToken = Guid.Parse("390AF1AF-5D53-48F1-B943-DDFBCB43C82B"),
                CreatedAt = DateTime.Parse("2026-09-30 03:41:32.8614085"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:41:32.8614085"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("65BDBC19-9125-426C-B792-6EB8E272D5A2"),
                ParentId = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                IsOn = true,
                Name = "Table2401a",
                Key = "MyBuilding.Floor24.Room2401.Table2401a",
                Description = "table #2401 a",
                ConcurrencyToken = Guid.Parse("2FA325D2-792D-4B6C-9615-6DD9C8BCF95C"),
                CreatedAt = DateTime.Parse("2026-09-30 03:42:10.6716511"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:42:10.6716511"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("04485343-B041-4603-82FF-D08B13292127"),
                ParentId = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                IsOn = true,
                Name = "Table2401b",
                Key = "MyBuilding.Floor24.Room2401.Table2401b",
                Description = "table #2401 b",
                ConcurrencyToken = Guid.Parse("411611E3-E849-4B78-8092-AD8FFFAE3C6C"),
                CreatedAt = DateTime.Parse("2026-09-30 03:42:13.7318341"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:42:13.7318341"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("9978B252-97BE-429C-B7A8-63F6579373E6"),
                ParentId = Guid.Parse("58E492D1-5723-427A-883F-5F5F6B661210"),
                IsOn = true,
                Name = "Table2401c",
                Key = "MyBuilding.Floor24.Room2401.Table2401c",
                Description = "table #2401 c",
                ConcurrencyToken = Guid.Parse("84A515F0-6FAC-4EA3-A799-898F1D006CD5"),
                CreatedAt = DateTime.Parse("2026-09-30 03:42:16.7083398"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:42:16.7083398"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("DCCA661B-98A2-4FDF-8219-8B7A340A3688"),
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                IsOn = true,
                Name = "Room2402",
                Key = "MyBuilding.Floor24.Room2402",
                Description = "room #2402",
                ConcurrencyToken = Guid.Parse("D1B85423-A8AB-46BE-B726-7D6F5875AE86"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:00.2007052"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:00.2007052"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD"),
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                IsOn = true,
                Name = "Room2403",
                Key = "MyBuilding.Floor24.Room2403",
                Description = "room #2403",
                ConcurrencyToken = Guid.Parse("985E8726-E0BF-4BC8-8086-E99B101D00A5"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:19.7272716"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:19.7272716"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("A63D23CC-3A85-4B59-A420-CBFD1E127FC5"),
                ParentId = Guid.Parse("F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD"),
                IsOn = true,
                Name = "Table2403a",
                Key = "MyBuilding.Floor24.Room2403.Table2403a",
                Description = "table #2403 a",
                ConcurrencyToken = Guid.Parse("7A8C9CDA-E5C3-4B89-B708-FFACBD29A3BD"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:49.1854194"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:49.1854194"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("09A4D20D-B63F-4120-BC95-9A0A6532ADD1"),
                ParentId = Guid.Parse("F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD"),
                IsOn = true,
                Name = "Table2403b",
                Key = "MyBuilding.Floor24.Room2403.Table2403b",
                Description = "table #2403 b",
                ConcurrencyToken = Guid.Parse("A3D1C2E4-A681-463F-AF49-131289720675"),
                CreatedAt = DateTime.Parse("2026-09-30 03:43:52.2221069"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:43:52.2221069"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("56CF92D4-0FFF-40D5-B7B4-E78E03961650"),
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                IsOn = true,
                Name = "Room2404",
                Key = "MyBuilding.Floor24.Room2404",
                Description = "room #2404",
                ConcurrencyToken = Guid.Parse("AE25278C-7AD1-48E8-BDBC-7DC5529C5899"),
                CreatedAt = DateTime.Parse("2026-09-30 03:44:24.5148005"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:44:24.5148005"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("06DD9F21-626B-4B88-93F7-BE8B2E4FC3F0"),
                ParentId = Guid.Parse("404FA049-CBB7-4C6E-A143-3340678CA90E"),
                IsOn = true,
                Name = "Room2405",
                Key = "MyBuilding.Floor24.Room2405",
                Description = "room #2405",
                ConcurrencyToken = Guid.Parse("720EA77C-52ED-4815-B1CF-AC06C5705E3C"),
                CreatedAt = DateTime.Parse("2026-09-30 03:44:33.9484467"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:44:33.9484467"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("52B40BAA-183B-4C6E-A8E9-71AF2F5F797F"),
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                IsOn = true,
                Name = "Floor25",
                Key = "MyBuilding.Floor25",
                Description = "floor #25",
                ConcurrencyToken = Guid.Parse("D066581F-4566-41EC-8CEE-75AB2519E26C"),
                CreatedAt = DateTime.Parse("2026-09-30 03:45:23.2836617"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:45:23.2836617"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6"),
                ParentId = Guid.Parse("4F228BF5-BBB0-428C-92CE-B63754579712"),
                IsOn = true,
                Name = "Floor26",
                Key = "MyBuilding.Floor26",
                Description = "floor #26",
                ConcurrencyToken = Guid.Parse("06C2F42C-6BC6-4DD8-A81F-279F4F194F92"),
                CreatedAt = DateTime.Parse("2026-09-30 03:45:45.9977765"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:45:45.9977765"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("1AEE1C30-B4BA-4B5D-9BBE-0078C372579F"),
                ParentId = Guid.Parse("10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6"),
                IsOn = true,
                Name = "Room2601",
                Key = "MyBuilding.Floor26.Room2601",
                Description = "room #2601",
                ConcurrencyToken = Guid.Parse("77E5F5EC-43AB-4EA8-8808-449FA24B57B1"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:10.4785162"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:10.4785162"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                ParentId = Guid.Parse("10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6"),
                IsOn = true,
                Name = "Room2602",
                Key = "MyBuilding.Floor26.Room2602",
                Description = "room #2602",
                ConcurrencyToken = Guid.Parse("76B48458-58BB-4445-97C2-0FE10F94C168"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:17.4000942"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:17.4000942"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("9B503318-BDB1-4EA7-8D2D-306C1E974E70"),
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                IsOn = true,
                Name = "Table2602a",
                Key = "MyBuilding.Floor26.Room2602.Table2602a",
                Description = "table #2602 a",
                ConcurrencyToken = Guid.Parse("741BA645-DC05-490A-97C7-F403EA559DC4"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:38.2760252"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:38.2760252"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("1430860F-FEA7-4318-BE0D-5B78157FA328"),
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                IsOn = true,
                Name = "Table2602b",
                Key = "MyBuilding.Floor26.Room2602.Table2602b",
                Description = "table #2602 b",
                ConcurrencyToken = Guid.Parse("FB0CF018-D4AA-4652-BFCB-1790EC9F7B0B"),
                CreatedAt = DateTime.Parse("2026-09-30 03:46:44.7748539"),
                UpdatedAt = DateTime.Parse("2026-09-30 03:46:44.7748539"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("DBE0F6F1-9338-497A-998C-234AD831CFB6"),
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                IsOn = true,
                Name = "Table2602c",
                Key = "MyBuilding.Floor26.Room2602.Table2602c",
                Description = "table #2602 c",
                ConcurrencyToken = Guid.Parse("3EC5281B-EE4D-410B-B477-BD8D4CE9FD81"),
                CreatedAt = DateTime.Parse("2026-10-17 20:36:30.3876071"),
                UpdatedAt = DateTime.Parse("2026-10-17 20:36:30.3876071"),
                DeletedAt = null,
            };

        yield return
            new Flag
            {
                Id = Guid.Parse("58934C81-05C6-4E0B-A62D-24996957D486"),
                ParentId = Guid.Parse("CE0DEB00-7946-4BE0-AE93-7E1B42178841"),
                IsOn = true,
                Name = "Table2602d",
                Key = "MyBuilding.Floor26.Room2602.Table2602d",
                Description = "table #2602 d",
                ConcurrencyToken = Guid.Parse("4F8F0842-4B70-4C3F-9B29-8DBDAC978268"),
                CreatedAt = DateTime.Parse("2026-10-17 20:36:30.3876072"),
                UpdatedAt = DateTime.Parse("2026-10-17 20:36:30.3876072"),
                DeletedAt = null,
            };
    }
}
