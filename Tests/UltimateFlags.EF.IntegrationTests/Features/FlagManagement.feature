Feature: Flag Management

@ResetBasicFlags
Scenario Outline: 1. Update Flag

    When I update a flag with the Id, <Id>, as follows
        | IsOn   | Description   |
        | <IsOn> | <Description> |
    Then flag data with the Id, <Id>, is updated properly
        | IsOn   | Description   |
        | <IsOn> | <Description> |

Examples:
    | Id                                   | IsOn  | Description                 |
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | false | any randome string %^!*$#~  |
    | A63D23CC-3A85-4B59-A420-CBFD1E127FC5 | true  | any randome string 123613   |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | false | yet another random words ~~ |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | true  | ~~ yet another random words |

@ResetBasicFlags
Scenario Outline: 2. Delete Flag

    When I delete a flag with the Id, <Id>
    Then The flags with the following Ids have been deleted, '<DeletedIdsString>'

Examples:
    | Id                                   | DeletedIdsString                                                                                                                                                                                                                                                   |
    | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD | F9DB6BEE-5B40-4051-930A-2ACBEBAD36CD,A63D23CC-3A85-4B59-A420-CBFD1E127FC5,09A4D20D-B63F-4120-BC95-9A0A6532ADD1                                                                                                                                                     |
    | CDFA551C-5504-4475-97BE-66A9084C81A7 | CDFA551C-5504-4475-97BE-66A9084C81A7,12A46EC8-AFFB-4E6A-BABF-D1F92E661FE5,6F3BD2C8-F17C-4903-B3BE-C1CA88EABCCE,6CA466A8-11E3-4751-B69A-D06714CCEEA1,705FA822-E54B-4505-87B5-5EDC1E69D780                                                                           |
    | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F | 1AEE1C30-B4BA-4B5D-9BBE-0078C372579F                                                                                                                                                                                                                               |
    | 04485343-B041-4603-82FF-D08B13292127 | 04485343-B041-4603-82FF-D08B13292127                                                                                                                                                                                                                               |
    | 52B40BAA-183B-4C6E-A8E9-71AF2F5F797F | 52B40BAA-183B-4C6E-A8E9-71AF2F5F797F                                                                                                                                                                                                                               |
    | CE0DEB00-7946-4BE0-AE93-7E1B42178841 | CE0DEB00-7946-4BE0-AE93-7E1B42178841,9B503318-BDB1-4EA7-8D2D-306C1E974E70,1430860F-FEA7-4318-BE0D-5B78157FA328,DBE0F6F1-9338-497A-998C-234AD831CFB6,58934C81-05C6-4E0B-A62D-24996957D486                                                                           |
    | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6 | 10EC94AD-9D9E-49A5-AAE8-D0C5FA93A2E6,1AEE1C30-B4BA-4B5D-9BBE-0078C372579F,CE0DEB00-7946-4BE0-AE93-7E1B42178841,9B503318-BDB1-4EA7-8D2D-306C1E974E70,1430860F-FEA7-4318-BE0D-5B78157FA328,DBE0F6F1-9338-497A-998C-234AD831CFB6,58934C81-05C6-4E0B-A62D-24996957D486 |
