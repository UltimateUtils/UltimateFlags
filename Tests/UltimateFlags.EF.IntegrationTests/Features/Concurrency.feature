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
    Then flag data with the Id, <Id>, is updated properly
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
