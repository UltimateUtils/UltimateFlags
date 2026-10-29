Feature: Optimistic Concurrency Control feature

Scenario: 1. Update Flag
    Given DB has the following flags
        | Id                                   | Name       | Key        | IsOn | Description | ParentId | CreatedAt                   | UpdatedAt                   | DeletedAt |
        | 4F228BF5-BBB0-428C-92CE-B63754579712 | MyBuilding | MyBuilding | TRUE | my building | <null>   | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>    |
    When I update a flag which has been updated by another process
        | Id                                   | Description       |
        | 4F228BF5-BBB0-428C-92CE-B63754579712 | my small building |
    Then ConcurrencyConflict exception is thrown
