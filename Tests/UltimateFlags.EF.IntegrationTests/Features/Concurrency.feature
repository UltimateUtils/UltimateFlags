Feature: Concurrency feature

@ResetBasicFlags
Scenario Outline: 1. Update Flag - null Concurrency Token

    When I update a flag with the Id, <Id>, as follows
        | IsOn   | Description   | ConcurrencyToken   |
        | <IsOn> | <Description> | <ConcurrencyToken> |
    Then flag data with the Id, <Id>, is updated properly
        | IsOn   | Description   |
        | <IsOn> | <Description> |

Examples:
    | Id                                   | IsOn  | Description                 | ConcurrencyToken |
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | false | any randome string %^!*$#~  | <null>           |
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | true  | any randome string 123613   | <null>           |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | false | yet another random words ~~ | <null>           |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | true  | ~~ yet another random words | <null>           |

@ResetBasicFlags
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
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | false | any randome string %^!*$#~  | 7A8C9CDA-E5C3-4B89-B708-FFACBD29A3BD |
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | true  | any randome string 123613   | 7A8C9CDA-E5C3-4B89-B708-FFACBD29A3BD |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | false | yet another random words ~~ | 77E5F5EC-43AB-4EA8-8808-449FA24B57B1 |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | true  | ~~ yet another random words | 77E5F5EC-43AB-4EA8-8808-449FA24B57B1 |

@ResetBasicFlags
Scenario Outline: 3. Update Flag - Incorrect Concurrency Token

    Given I have retrieved a flag, <Id>
    When I update the retrieved flag with the following properties
        | IsOn   | Description   | ConcurrencyToken   |
        | <IsOn> | <Description> | <ConcurrencyToken> |
    Then ConcurrencyConflict exception is thrown

Examples:
    | Id                                   | IsOn  | Description                 | ConcurrencyToken                     |
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | false | any randome string %^!*$#~  | 03FEA905-C322-4FE6-AC5A-F725F8AB92A5 |
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | true  | any randome string 123613   | 92412056-19D5-41D4-8837-EA05CD402A10 |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | false | yet another random words ~~ | 1ECA6C71-1A84-4C79-9113-DF65998EAEA7 |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | true  | ~~ yet another random words | A229739E-909C-43AD-8545-4D55BA9092A2 |

@ResetBasicFlags
Scenario Outline: 4. Delete Flag - Correct Concurrency Token

    Given I have retrieved a flag, <Id>
    When I delete a flag, <Id>, with a ConcurrencyToken, <ConcurrencyToken>
    Then no exception is thrown
    And exactly the flags with the following Ids are deleted, '<DeletedIdsString>'

Examples:
    | Id                                   | ConcurrencyToken                     | DeletedIdsString                                                                                                                                                                                                                                                   |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | 77E5F5EC-43AB-4EA8-8808-449FA24B57B1 | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F                                                                                                                                                                                                                               |
    | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | 06C2F42C-6BC6-4DD8-A81F-279F4F194F92 | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6,1AEE1C30-B4BA-4B5D-9BBE-0078C372579F,CE0DEB00-7946-4BE0-AE93-7E1B42178841,9B503318-BDB1-4EA7-8D2D-306C1E974E70,1430860F-FEA7-4318-BE0D-5B78157FA328,DBE0F6F1-9338-497A-998C-234AD831CFB6,58934C81-05C6-4E0B-A62D-24996957D486 |

@ResetBasicFlags
Scenario Outline: 5. Delete Flag - Incorrect Concurrency Token

    Given I have retrieved a flag, <Id>
    When I delete a flag, <Id>, with a ConcurrencyToken, <ConcurrencyToken>
    Then ConcurrencyConflict exception is thrown
    And the flags with the following Ids are not deleted, '<DeletedIdsString>'

Examples:
    | Id                                   | ConcurrencyToken                     | DeletedIdsString                                                                                                                                                                                                                                                   |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | ABCDEF12-3456-7890-ABCD-EF1234567890 | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F                                                                                                                                                                                                                               |
    | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | ABCDEF12-3456-7890-ABCD-EF1234567890 | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6,1AEE1C30-B4BA-4B5D-9BBE-0078C372579F,CE0DEB00-7946-4BE0-AE93-7E1B42178841,9B503318-BDB1-4EA7-8D2D-306C1E974E70,1430860F-FEA7-4318-BE0D-5B78157FA328,DBE0F6F1-9338-497A-998C-234AD831CFB6,58934C81-05C6-4E0B-A62D-24996957D486 |

@ResetBasicFlags
Scenario Outline: 6. Purge Flag - Correct Concurrency Token

    Given the flag, <Id>, has been deleted
    When I purge the flag, <Id>, with the correct concurrency token
    Then no exception is thrown
    And the flags, '<PurgedIdsString>', are purged
    And no flags left soft-deleted

Examples:
    | Id                                   | PurgedIdsString                                                                                                                                                                                                                                                    |
    | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD,A63D23CC-3A85-4B59-A420-CBFD1E127FC5,09A4D20D-B63F-4120-BC95-9A0A6532ADD1                                                                                                                                                     |
    | CDFA551C-5504-4475-97BE-66A9084C81A7 | CDFA551C-5504-4475-97BE-66A9084C81A7,12A46EC8-AFFB-4E6A-BABF-D1F92E661FE5,6F3BD2C8-F17C-4903-B3BE-C1CA88EABCCE,6CA466A8-11E3-4751-B69A-D06714CCEEA1,705FA822-E54B-4505-87B5-5EDC1E69D780                                                                           |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F                                                                                                                                                                                                                               |
    | 04485343-B041-4603-82FF-D08B13292127 | 04485343-B041-4603-82FF-D08B13292127                                                                                                                                                                                                                               |
    | 52B40BAA-183B-4C6E-A8E9-71AF2F5F797F | 52B40BAA-183B-4C6E-A8E9-71AF2F5F797F                                                                                                                                                                                                                               |
    | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | CE0DEB00-7946-4BE0-AE93-7E1B42178841,9B503318-BDB1-4EA7-8D2D-306C1E974E70,1430860F-FEA7-4318-BE0D-5B78157FA328,DBE0F6F1-9338-497A-998C-234AD831CFB6,58934C81-05C6-4E0B-A62D-24996957D486                                                                           |
    | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6,1AEE1C30-B4BA-4B5D-9BBE-0078C372579F,CE0DEB00-7946-4BE0-AE93-7E1B42178841,9B503318-BDB1-4EA7-8D2D-306C1E974E70,1430860F-FEA7-4318-BE0D-5B78157FA328,DBE0F6F1-9338-497A-998C-234AD831CFB6,58934C81-05C6-4E0B-A62D-24996957D486 |

@ResetBasicFlags
Scenario Outline: 7. Purge Flag - Incorrect Concurrency Token

    Given the flag, <Id>, has been deleted
    When I purge the flag, <Id>, with a concurrency token, <ConcurrencyToken>
    Then ConcurrencyConflict exception is thrown
    And the flags, '<DeletedIdsString>', left soft-deleted

Examples:
    | Id                                   | ConcurrencyToken                     | DeletedIdsString                                                                                                                                                                                                                                                   |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | ABCDEF12-3456-7890-ABCD-EF1234567890 | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F                                                                                                                                                                                                                               |
    | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | ABCDEF12-3456-7890-ABCD-EF1234567890 | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6,1AEE1C30-B4BA-4B5D-9BBE-0078C372579F,CE0DEB00-7946-4BE0-AE93-7E1B42178841,9B503318-BDB1-4EA7-8D2D-306C1E974E70,1430860F-FEA7-4318-BE0D-5B78157FA328,DBE0F6F1-9338-497A-998C-234AD831CFB6,58934C81-05C6-4E0B-A62D-24996957D486 |
