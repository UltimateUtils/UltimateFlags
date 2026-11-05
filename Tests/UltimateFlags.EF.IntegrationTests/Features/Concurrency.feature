Feature: Concurrency feature

@BeforeScenarioResetBasicFlags
Scenario Outline: 1. Update Flag - null Concurrency Token

    When I update a flag with the Id, <Id>, as follows
        | IsOn   | Description   | ConcurrencyToken   |
        | <IsOn> | <Description> | <ConcurrencyToken> |
    Then flag data with the Id, <Id>, is updated properly
        | IsOn   | Description   |
        | <IsOn> | <Description> |

Examples:
    | Id                                   | IsOn  | Description                 | ConcurrencyToken |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | false | any randome string %^!*$#~  | <null>           |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | true  | any randome string 123613   | <null>           |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | false | yet another random words ~~ | <null>           |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | true  | ~~ yet another random words | <null>           |

@BeforeScenarioResetBasicFlags
Scenario Outline: 2. Update Flag - Correct Concurrency Token

    Given I have retrieved a flag, <Id>
    When I update the retrieved flag with the following properties
        | IsOn   | Description   | ConcurrencyToken   |
        | <IsOn> | <Description> | <ConcurrencyToken> |
    Then no exception is thrown
    And flag data with the Id, <Id>, is updated properly
        | IsOn   | Description   |
        | <IsOn> | <Description> |

Examples:
    | Id                                   | IsOn  | Description                 | ConcurrencyToken                     |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | false | any randome string %^!*$#~  | 7A8C9CDA-E5C3-4B89-B708-FFACBD29A3BD |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | true  | any randome string 123613   | 7A8C9CDA-E5C3-4B89-B708-FFACBD29A3BD |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | false | yet another random words ~~ | 77E5F5EC-43AB-4EA8-8808-449FA24B57B1 |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | true  | ~~ yet another random words | 77E5F5EC-43AB-4EA8-8808-449FA24B57B1 |

@BeforeScenarioResetBasicFlags
Scenario Outline: 3. Update Flag - Incorrect Concurrency Token

    Given I have retrieved a flag, <Id>
    When I update the retrieved flag with the following properties
        | IsOn   | Description   | ConcurrencyToken   |
        | <IsOn> | <Description> | <ConcurrencyToken> |
    Then ConcurrencyConflict exception is thrown

Examples:
    | Id                                   | IsOn  | Description                 | ConcurrencyToken                     |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | false | any randome string %^!*$#~  | 03FEA905-C322-4FE6-AC5A-F725F8AB92A5 |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | true  | any randome string 123613   | 92412056-19D5-41D4-8837-EA05CD402A10 |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | false | yet another random words ~~ | 1ECA6C71-1A84-4C79-9113-DF65998EAEA7 |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | true  | ~~ yet another random words | A229739E-909C-43AD-8545-4D55BA9092A2 |

@BeforeScenarioResetBasicFlags
Scenario Outline: 4. Delete Flag - Correct Concurrency Token

    Given I have retrieved a flag, <Id>
    When I delete a flag, <Id>, with a ConcurrencyToken, <ConcurrencyToken>
    Then no exception is thrown
    And exactly the flags with the following Ids are deleted, '<DeletedIdsString>'

Examples:
    | Id                                   | ConcurrencyToken                     | DeletedIdsString                                                                                                                                                                                                                                                   |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | 77E5F5EC-43AB-4EA8-8808-449FA24B57B1 | 01A1A96C-2145-7840-88C7-A03F45928635                                                                                                                                                                                                                               |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | 06C2F42C-6BC6-4DD8-A81F-279F4F194F92 | 01A1A96C-0709-7A4E-831F-FB154377E39D,01A1A96C-2145-7840-88C7-A03F45928635,01A1A96C-86DD-74B3-BF66-7641B575DA9B,01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9,01A1A96C-F21A-7D7F-B77B-79BB617C30ED,01A1A96D-000F-7AAA-9517-0E9A9C6528E7,01A1A96D-3A5D-7188-B085-19561F94D850 |

@BeforeScenarioResetBasicFlags
Scenario Outline: 5. Delete Flag - Incorrect Concurrency Token

    Given I have retrieved a flag, <Id>
    When I delete a flag, <Id>, with a ConcurrencyToken, <ConcurrencyToken>
    Then ConcurrencyConflict exception is thrown
    And the flags with the following Ids are not deleted, '<DeletedIdsString>'

Examples:
    | Id                                   | ConcurrencyToken                     | DeletedIdsString                                                                                                                                                                                                                                                   |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | ABCDEF12-3456-7890-ABCD-EF1234567890 | 01A1A96C-2145-7840-88C7-A03F45928635                                                                                                                                                                                                                               |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | ABCDEF12-3456-7890-ABCD-EF1234567890 | 01A1A96C-0709-7A4E-831F-FB154377E39D,01A1A96C-2145-7840-88C7-A03F45928635,01A1A96C-86DD-74B3-BF66-7641B575DA9B,01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9,01A1A96C-F21A-7D7F-B77B-79BB617C30ED,01A1A96D-000F-7AAA-9517-0E9A9C6528E7,01A1A96D-3A5D-7188-B085-19561F94D850 |

@BeforeScenarioResetBasicFlags
Scenario Outline: 6. Purge Flag - Correct Concurrency Token

    Given the flag, <Id>, has been deleted
    When I purge the flag, <Id>, with the correct concurrency token
    Then no exception is thrown
    And the flags, '<PurgedIdsString>', are purged
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
Scenario Outline: 7. Purge Flag - Incorrect Concurrency Token

    Given the flag, <Id>, has been deleted
    When I purge the flag, <Id>, with a concurrency token, <ConcurrencyToken>
    Then ConcurrencyConflict exception is thrown
    And the flags, '<DeletedIdsString>', left soft-deleted

Examples:
    | Id                                   | ConcurrencyToken                     | DeletedIdsString                                                                                                                                                                                                                                                   |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | ABCDEF12-3456-7890-ABCD-EF1234567890 | 01A1A96C-2145-7840-88C7-A03F45928635                                                                                                                                                                                                                               |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | ABCDEF12-3456-7890-ABCD-EF1234567890 | 01A1A96C-0709-7A4E-831F-FB154377E39D,01A1A96C-2145-7840-88C7-A03F45928635,01A1A96C-86DD-74B3-BF66-7641B575DA9B,01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9,01A1A96C-F21A-7D7F-B77B-79BB617C30ED,01A1A96D-000F-7AAA-9517-0E9A9C6528E7,01A1A96D-3A5D-7188-B085-19561F94D850 |
