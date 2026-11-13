Feature: IsOn Cache Feature

@BeforeScenarioResetBasicFlags
Scenario Outline: 1. IsOn

    Given cache has been cleared
    When I call IsOn cache by key, '<Key>'
    Then the response is <IsOn>

Examples:
    | Key                                    | IsOn |
    | MyBuilding                             | TRUE |
    | MyBuilding.Floor23                     | TRUE |
    | MyBuilding.Floor23.Room2301            | TRUE |
    | MyBuilding.Floor23.Room2302            | TRUE |
    | MyBuilding.Floor23.Room2303            | TRUE |
    | MyBuilding.Floor23.Room2304            | TRUE |
    | MyBuilding.Floor24                     | TRUE |
    | MyBuilding.Floor24.Room2401            | TRUE |
    | MyBuilding.Floor24.Room2401.Table2401a | TRUE |
    | MyBuilding.Floor24.Room2401.Table2401b | TRUE |
    | MyBuilding.Floor24.Room2401.Table2401c | TRUE |
    | MyBuilding.Floor24.Room2402            | TRUE |
    | MyBuilding.Floor24.Room2403            | TRUE |
    | MyBuilding.Floor24.Room2403.Table2403a | TRUE |
    | MyBuilding.Floor24.Room2403.Table2403b | TRUE |
    | MyBuilding.Floor24.Room2404            | TRUE |
    | MyBuilding.Floor24.Room2405            | TRUE |
    | MyBuilding.Floor25                     | TRUE |
    | MyBuilding.Floor26                     | TRUE |
    | MyBuilding.Floor26.Room2601            | TRUE |
    | MyBuilding.Floor26.Room2602            | TRUE |
    | MyBuilding.Floor26.Room2602.Table2602a | TRUE |
    | MyBuilding.Floor26.Room2602.Table2602b | TRUE |
    | MyBuilding.Floor26.Room2602.Table2602c | TRUE |
    | MyBuilding.Floor26.Room2602.Table2602d | TRUE |

@BeforeScenarioResetBasicFlags
Scenario Outline: 2. IsOn/Off

    Given I have turned off the following flags
        | Id                                   |
        | 01A1A968-66A6-7189-A0DD-95C844F16129 |
        | 01A1A96B-9FAF-76FB-9661-4360CEB07A73 |
        | 01A1A96C-86DD-74B3-BF66-7641B575DA9B |
    And cache has been cleared
    When I call IsOn cache by key, '<Key>'
    Then the response is <IsOn>

Examples:
    | Key                                    | IsOn  |
    | MyBuilding                             | TRUE  |
    | MyBuilding.Floor23                     | FALSE |
    | MyBuilding.Floor23.Room2301            | FALSE |
    | MyBuilding.Floor23.Room2302            | FALSE |
    | MyBuilding.Floor23.Room2303            | FALSE |
    | MyBuilding.Floor23.Room2304            | FALSE |
    | MyBuilding.Floor24                     | TRUE  |
    | MyBuilding.Floor24.Room2401            | TRUE  |
    | MyBuilding.Floor24.Room2401.Table2401a | TRUE  |
    | MyBuilding.Floor24.Room2401.Table2401b | TRUE  |
    | MyBuilding.Floor24.Room2401.Table2401c | TRUE  |
    | MyBuilding.Floor24.Room2402            | TRUE  |
    | MyBuilding.Floor24.Room2403            | TRUE  |
    | MyBuilding.Floor24.Room2403.Table2403a | TRUE  |
    | MyBuilding.Floor24.Room2403.Table2403b | TRUE  |
    | MyBuilding.Floor24.Room2404            | TRUE  |
    | MyBuilding.Floor24.Room2405            | TRUE  |
    | MyBuilding.Floor25                     | FALSE |
    | MyBuilding.Floor26                     | TRUE  |
    | MyBuilding.Floor26.Room2601            | TRUE  |
    | MyBuilding.Floor26.Room2602            | FALSE |
    | MyBuilding.Floor26.Room2602.Table2602a | FALSE |
    | MyBuilding.Floor26.Room2602.Table2602b | FALSE |
    | MyBuilding.Floor26.Room2602.Table2602c | FALSE |
    | MyBuilding.Floor26.Room2602.Table2602d | FALSE |
