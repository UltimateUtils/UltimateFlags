Feature: Basic Create feature

Scenario: 1. Create Flag
    Given DB has the following flags
        | Id                                   | Name       | Key                | IsOn | Description | ParentId                             | CreatedAt                   | UpdatedAt                   | DeletedAt |
        | 01A1A968-05EA-7445-9702-95FFA353EA91 | MyBuilding | MyBuilding         | TRUE | my building | <null>                               | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>    |
        | 01A1A968-66A6-7189-A0DD-95C844F16129 | Floor23    | MyBuilding.Floor23 | TRUE | floor #23   | 01A1A968-05EA-7445-9702-95FFA353EA91 | 2026-09-30 03:38:26.5139868 | 2026-09-30 03:38:26.5139868 | <null>    |
    When I create the following flag
        | Name     | IsOn  | Description | ParentId                             |
        | Room2301 | FALSE | room #2301  | 01A1A968-66A6-7189-A0DD-95C844F16129 |
    Then newly added flag is as follows
        | Name     | Key                         | IsOn  | Description | ParentId                             |
        | Room2301 | MyBuilding.Floor23.Room2301 | FALSE | room #2301  | 01A1A968-66A6-7189-A0DD-95C844F16129 |
