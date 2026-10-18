Feature: Flag Management

This feature is about flag management.

Scenario: Add Flag

    Given DB is ready
    When flag is added
    Then flag data is populated properly
