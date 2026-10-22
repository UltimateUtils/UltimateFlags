@AddBasicFlags
Feature: Flag Management

Scenario Outline: Update Flag

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
