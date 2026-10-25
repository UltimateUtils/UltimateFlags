Feature: Basic Create feature

Scenario: 1. Create Flags
    Given DB has the following flags
        | Id                                   | Name       | Key                | IsOn | Description | ParentId                             | CreatedAt                   | UpdatedAt                   | DeletedAt |
        | 4F228BF5-BBB0-428C-92CE-B63754579712 | MyBuilding | MyBuilding         | TRUE | my building | <null>                               | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>    |
        | CDFA551C-5504-4475-97BE-66A9084C81A7 | Floor23    | MyBuilding.Floor23 | TRUE | floor #23   | 4F228BF5-BBB0-428C-92CE-B63754579712 | 2026-09-30 03:38:26.5139868 | 2026-09-30 03:38:26.5139868 | <null>    |
    When I create the following flag
        | Name     | IsOn  | Description | ParentId                             |
        | Room2301 | FALSE | room #2301  | CDFA551C-5504-4475-97BE-66A9084C81A7 |
    Then newly added flag is as follows
        | Name     | Key                         | IsOn  | Description | ParentId                             |
        | Room2301 | MyBuilding.Floor23.Room2301 | FALSE | room #2301  | CDFA551C-5504-4475-97BE-66A9084C81A7 |
