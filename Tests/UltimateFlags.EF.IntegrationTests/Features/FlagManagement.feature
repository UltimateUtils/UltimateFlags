Feature: Flag Management Feature

@BeforeScenarioResetBasicFlags
Scenario Outline: 1. Update Flag

    When I update a flag with the Id, <Id>, as follows
        | IsOn   | Description   |
        | <IsOn> | <Description> |
    Then flag data with the Id, <Id>, is updated properly
        | IsOn   | Description   |
        | <IsOn> | <Description> |

Examples:
    | Id                                   | IsOn  | Description                 |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | false | any randome string %^!*$#~  |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | true  | any randome string 123613   |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | false | yet another random words ~~ |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | true  | ~~ yet another random words |

@BeforeScenarioResetBasicFlags
Scenario Outline: 2. Delete Flag

    When I delete a flag with the Id, <Id>
    Then exactly the flags with the following Ids are deleted, '<DeletedIdsString>'

Examples:
    | Id                                   | DeletedIdsString                                                                                                                                                                                                                                                   |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA,01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90,01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2                                                                                                                                                     |
    | 01A1A968-66A6-7189-A0DD-95C844F16129 | 01A1A968-66A6-7189-A0DD-95C844F16129,01A1A968-9C22-77AF-9C71-14455B6B3F66,01A1A968-CB3C-7BBB-96D1-537EFB7333A9,01A1A969-105B-7F0E-9AB4-19E973672346,01A1A969-5107-7075-A5F9-9ED622B2AAEB                                                                           |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | 01A1A96C-2145-7840-88C7-A03F45928635                                                                                                                                                                                                                               |
    | 01A1A96A-27E0-7F76-B371-57E854A1675A | 01A1A96A-27E0-7F76-B371-57E854A1675A                                                                                                                                                                                                                               |
    | 01A1A96B-9FAF-76FB-9661-4360CEB07A73 | 01A1A96B-9FAF-76FB-9661-4360CEB07A73                                                                                                                                                                                                                               |
    | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | 01A1A96C-86DD-74B3-BF66-7641B575DA9B,01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9,01A1A96C-F21A-7D7F-B77B-79BB617C30ED,01A1A96D-000F-7AAA-9517-0E9A9C6528E7,01A1A96D-3A5D-7188-B085-19561F94D850                                                                           |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | 01A1A96C-0709-7A4E-831F-FB154377E39D,01A1A96C-2145-7840-88C7-A03F45928635,01A1A96C-86DD-74B3-BF66-7641B575DA9B,01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9,01A1A96C-F21A-7D7F-B77B-79BB617C30ED,01A1A96D-000F-7AAA-9517-0E9A9C6528E7,01A1A96D-3A5D-7188-B085-19561F94D850 |

@BeforeScenarioResetBasicFlags
Scenario Outline: 3. Purge Flag

    Given the flag, <Id>, has been deleted
    When I purge the flag with the Id, <Id>
    Then the flags, '<PurgedIdsString>', are purged
    And no flags left soft-deleted

Examples:
    | Id                                   | PurgedIdsString                                                                                                                                                                                                                                                    |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA,01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90,01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2                                                                                                                                                     |
    | 01A1A968-66A6-7189-A0DD-95C844F16129 | 01A1A968-66A6-7189-A0DD-95C844F16129,01A1A968-9C22-77AF-9C71-14455B6B3F66,01A1A968-CB3C-7BBB-96D1-537EFB7333A9,01A1A969-105B-7F0E-9AB4-19E973672346,01A1A969-5107-7075-A5F9-9ED622B2AAEB                                                                           |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | 01A1A96C-2145-7840-88C7-A03F45928635                                                                                                                                                                                                                               |
    | 01A1A96A-27E0-7F76-B371-57E854A1675A | 01A1A96A-27E0-7F76-B371-57E854A1675A                                                                                                                                                                                                                               |
    | 01A1A96B-9FAF-76FB-9661-4360CEB07A73 | 01A1A96B-9FAF-76FB-9661-4360CEB07A73                                                                                                                                                                                                                               |
    | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | 01A1A96C-86DD-74B3-BF66-7641B575DA9B,01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9,01A1A96C-F21A-7D7F-B77B-79BB617C30ED,01A1A96D-000F-7AAA-9517-0E9A9C6528E7,01A1A96D-3A5D-7188-B085-19561F94D850                                                                           |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | 01A1A96C-0709-7A4E-831F-FB154377E39D,01A1A96C-2145-7840-88C7-A03F45928635,01A1A96C-86DD-74B3-BF66-7641B575DA9B,01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9,01A1A96C-F21A-7D7F-B77B-79BB617C30ED,01A1A96D-000F-7AAA-9517-0E9A9C6528E7,01A1A96D-3A5D-7188-B085-19561F94D850 |

@BeforeScenarioResetBasicFlags
Scenario Outline: 4. Purge Flag 2

    Given the flag, <Id>, has been deleted
    When I purge the flag with the Id, <IdToPurge>
    Then the flags, '<PurgedIdsString>', are purged
    And the flags, '<DeletedIdsString>', left soft-deleted

Examples:
    | Id                                   | IdToPurge                            | PurgedIdsString                                                                                                                                     | DeletedIdsString                                                                                                                                                                                                                                                                                        |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90                                                                                                                | 01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2                                                                                                                                                                                                                                                                    |
    | 01A1A969-721E-713C-B658-1782772551C2 | 01A1A969-B33C-791C-8E3B-F770A0227ADA | 01A1A969-B33C-791C-8E3B-F770A0227ADA,01A1A96A-109B-7B54-976C-324E4484F7A1,01A1A96A-27E0-7F76-B371-57E854A1675A,01A1A96A-4DD2-73E7-8851-A9D5079B1E45 | 01A1A969-721E-713C-B658-1782772551C2,01A1A96A-82B1-75B4-BD87-9894CE2E36B8,01A1A96A-8CE3-727B-8E5A-83A132AD39AA,01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90,01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2,01A1A96B-68FE-77FE-9BD8-C4C03814C7EF,01A1A96B-7D1E-7EFB-88F2-06ADE45F5888                                      |
    | 01A1A969-721E-713C-B658-1782772551C2 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA,01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90,01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2                                      | 01A1A969-721E-713C-B658-1782772551C2,01A1A969-B33C-791C-8E3B-F770A0227ADA,01A1A96A-109B-7B54-976C-324E4484F7A1,01A1A96A-27E0-7F76-B371-57E854A1675A,01A1A96A-4DD2-73E7-8851-A9D5079B1E45,01A1A96A-82B1-75B4-BD87-9894CE2E36B8,01A1A96B-68FE-77FE-9BD8-C4C03814C7EF,01A1A96B-7D1E-7EFB-88F2-06ADE45F5888 |

@BeforeScenarioResetBasicFlags
Scenario Outline: 5. Restore Flag

    Given the flag, <Id>, has been deleted
    When I restore the flag, <IdToRestore>
    Then the response has the flags, '<RestoredIds>'
    And the flags, '<RestoredIds>', are restored
    And the flags, '<DeletedIdsString>', left soft-deleted

Examples:
    | Id                                   | IdToRestore                          | RestoredIds                                                               | DeletedIdsString                                                          |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA                                      | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90,01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2 |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA,01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | 01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2                                      |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA,01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2 | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90                                      |
