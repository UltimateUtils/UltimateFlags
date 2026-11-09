Feature: Get Flag feature

Scenario Outline: 1. Get Flag by Id

    When retrieve a flag by <Id>
    Then the following flag is returned correctly
        | Id   | ParentId   | IsOn   | Name   | Key   | Description   | ConcurrencyToken   | CreatedAt   | UpdatedAt   | DeletedAt   |
        | <Id> | <ParentId> | <IsOn> | <Name> | <Key> | <Description> | <ConcurrencyToken> | <CreatedAt> | <UpdatedAt> | <DeletedAt> |

Examples:
    | Id                                   | ParentId                             | IsOn | Name       | Key                                    | Description   | CreatedAt                   | UpdatedAt                   | DeletedAt                   |
    | 01A1A968-05EA-7445-9702-95FFA353EA91 | <null>                               | TRUE | MyBuilding | MyBuilding                             | my building   | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>                      |
    | 01A1A968-66A6-7189-A0DD-95C844F16129 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor23    | MyBuilding.Floor23                     | floor #23     | 2026-09-30 03:38:26.5139868 | 2026-09-30 03:38:26.5139868 | 2026-10-07 14:25:27.1864023 |
    | 01A1A968-9C22-77AF-9C71-14455B6B3F66 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2301   | MyBuilding.Floor23.Room2301            | room #2301    | 2026-09-30 03:38:50.2655434 | 2026-09-30 03:38:50.2655434 | 2026-10-07 14:25:27.1864023 |
    | 01A1A968-CB3C-7BBB-96D1-537EFB7333A9 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2302   | MyBuilding.Floor23.Room2302            | room #2302    | 2026-09-30 03:38:58.7826112 | 2026-09-30 03:38:58.7826112 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-105B-7F0E-9AB4-19E973672346 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2303   | MyBuilding.Floor23.Room2303            | room #2303    | 2026-09-30 03:39:01.7470543 | 2026-09-30 03:39:01.7470543 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-5107-7075-A5F9-9ED622B2AAEB | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2304   | MyBuilding.Floor23.Room2304            | room #2304    | 2026-09-30 03:39:04.4400251 | 2026-09-30 03:39:04.4400251 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-721E-713C-B658-1782772551C2 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor24    | MyBuilding.Floor24                     | floor #24     | 2026-09-30 03:41:02.8863859 | 2026-09-30 03:41:02.8863859 | <null>                      |
    | 01A1A969-B33C-791C-8E3B-F770A0227ADA | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2401   | MyBuilding.Floor24.Room2401            | Room #2401    | 2026-09-30 03:41:32.8614085 | 2026-09-30 03:41:32.8614085 | <null>                      |
    | 01A1A96A-109B-7B54-976C-324E4484F7A1 | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401a | MyBuilding.Floor24.Room2401.Table2401a | table #2401 a | 2026-09-30 03:42:10.6716511 | 2026-09-30 03:42:10.6716511 | <null>                      |
    | 01A1A96A-27E0-7F76-B371-57E854A1675A | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401b | MyBuilding.Floor24.Room2401.Table2401b | table #2401 b | 2026-09-30 03:42:13.7318341 | 2026-09-30 03:42:13.7318341 | <null>                      |
    | 01A1A96A-4DD2-73E7-8851-A9D5079B1E45 | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401c | MyBuilding.Floor24.Room2401.Table2401c | table #2401 c | 2026-09-30 03:42:16.7083398 | 2026-09-30 03:42:16.7083398 | <null>                      |
    | 01A1A96A-82B1-75B4-BD87-9894CE2E36B8 | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2402   | MyBuilding.Floor24.Room2402            | room #2402    | 2026-09-30 03:43:00.2007052 | 2026-09-30 03:43:00.2007052 | <null>                      |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2403   | MyBuilding.Floor24.Room2403            | room #2403    | 2026-09-30 03:43:19.7272716 | 2026-09-30 03:43:19.7272716 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | TRUE | Table2403a | MyBuilding.Floor24.Room2403.Table2403a | table #2403 a | 2026-09-30 03:43:49.1854194 | 2026-09-30 03:43:49.1854194 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | TRUE | Table2403b | MyBuilding.Floor24.Room2403.Table2403b | table #2403 b | 2026-09-30 03:43:52.2221069 | 2026-09-30 03:43:52.2221069 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96B-68FE-77FE-9BD8-C4C03814C7EF | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2404   | MyBuilding.Floor24.Room2404            | room #2404    | 2026-09-30 03:44:24.5148005 | 2026-09-30 03:44:24.5148005 | <null>                      |
    | 01A1A96B-7D1E-7EFB-88F2-06ADE45F5888 | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2405   | MyBuilding.Floor24.Room2405            | room #2405    | 2026-09-30 03:44:33.9484467 | 2026-09-30 03:44:33.9484467 | <null>                      |
    | 01A1A96B-9FAF-76FB-9661-4360CEB07A73 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor25    | MyBuilding.Floor25                     | floor #25     | 2026-09-30 03:45:23.2836617 | 2026-09-30 03:45:23.2836617 | 2026-10-09 12:45:51.4011112 |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor26    | MyBuilding.Floor26                     | floor #26     | 2026-09-30 03:45:45.9977765 | 2026-09-30 03:45:45.9977765 | <null>                      |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | 01A1A96C-0709-7A4E-831F-FB154377E39D | TRUE | Room2601   | MyBuilding.Floor26.Room2601            | room #2601    | 2026-09-30 03:46:10.4785162 | 2026-09-30 03:46:10.4785162 | <null>                      |
    | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | 01A1A96C-0709-7A4E-831F-FB154377E39D | TRUE | Room2602   | MyBuilding.Floor26.Room2602            | room #2602    | 2026-09-30 03:46:17.4000942 | 2026-09-30 03:46:17.4000942 | <null>                      |
    | 01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602a | MyBuilding.Floor26.Room2602.Table2602a | table #2602 a | 2026-09-30 03:46:38.2760252 | 2026-09-30 03:46:38.2760252 | <null>                      |
    | 01A1A96C-F21A-7D7F-B77B-79BB617C30ED | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602b | MyBuilding.Floor26.Room2602.Table2602b | table #2602 b | 2026-09-30 03:46:44.7748539 | 2026-09-30 03:46:44.7748539 | <null>                      |
    | 01A1A96D-000F-7AAA-9517-0E9A9C6528E7 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602c | MyBuilding.Floor26.Room2602.Table2602c | table #2602 c | 2026-10-17 20:36:30.3876071 | 2026-10-17 20:36:30.3876071 | <null>                      |
    | 01A1A96D-3A5D-7188-B085-19561F94D850 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602d | MyBuilding.Floor26.Room2602.Table2602d | table #2602 d | 2026-10-17 20:36:30.3876072 | 2026-10-17 20:36:30.3876072 | <null>                      |

Scenario Outline: 2. Get Flag by incorrect Id

    When retrieve a flag by <Id>
    Then null is returned

Examples:
    | Id                                   |
    | 12345678-90AB-CDEF-1234-567890ABCDEF |
    | A554F8FA-0DE8-4899-B6BE-1CA92FFE1049 |
    | CA81F335-6C3D-4A6F-9E98-6AA998509EB7 |

Scenario Outline: 3. Get Flag by name

    When I retrieve a flag by '<Name>' and <ParentId>
    Then the following flag is returned correctly
        | Id   | ParentId   | IsOn   | Name   | Key   | Description   | ConcurrencyToken   | CreatedAt   | UpdatedAt   | DeletedAt   |
        | <Id> | <ParentId> | <IsOn> | <Name> | <Key> | <Description> | <ConcurrencyToken> | <CreatedAt> | <UpdatedAt> | <DeletedAt> |

Examples:
    | Id                                   | ParentId                             | IsOn | Name       | Key                                    | Description   | CreatedAt                   | UpdatedAt                   | DeletedAt                   |
    | 01A1A968-05EA-7445-9702-95FFA353EA91 | <null>                               | TRUE | MyBuilding | MyBuilding                             | my building   | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>                      |
    | 01A1A968-66A6-7189-A0DD-95C844F16129 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor23    | MyBuilding.Floor23                     | floor #23     | 2026-09-30 03:38:26.5139868 | 2026-09-30 03:38:26.5139868 | 2026-10-07 14:25:27.1864023 |
    | 01A1A968-9C22-77AF-9C71-14455B6B3F66 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2301   | MyBuilding.Floor23.Room2301            | room #2301    | 2026-09-30 03:38:50.2655434 | 2026-09-30 03:38:50.2655434 | 2026-10-07 14:25:27.1864023 |
    | 01A1A968-CB3C-7BBB-96D1-537EFB7333A9 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2302   | MyBuilding.Floor23.Room2302            | room #2302    | 2026-09-30 03:38:58.7826112 | 2026-09-30 03:38:58.7826112 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-105B-7F0E-9AB4-19E973672346 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2303   | MyBuilding.Floor23.Room2303            | room #2303    | 2026-09-30 03:39:01.7470543 | 2026-09-30 03:39:01.7470543 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-5107-7075-A5F9-9ED622B2AAEB | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2304   | MyBuilding.Floor23.Room2304            | room #2304    | 2026-09-30 03:39:04.4400251 | 2026-09-30 03:39:04.4400251 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-721E-713C-B658-1782772551C2 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor24    | MyBuilding.Floor24                     | floor #24     | 2026-09-30 03:41:02.8863859 | 2026-09-30 03:41:02.8863859 | <null>                      |
    | 01A1A969-B33C-791C-8E3B-F770A0227ADA | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2401   | MyBuilding.Floor24.Room2401            | Room #2401    | 2026-09-30 03:41:32.8614085 | 2026-09-30 03:41:32.8614085 | <null>                      |
    | 01A1A96A-109B-7B54-976C-324E4484F7A1 | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401a | MyBuilding.Floor24.Room2401.Table2401a | table #2401 a | 2026-09-30 03:42:10.6716511 | 2026-09-30 03:42:10.6716511 | <null>                      |
    | 01A1A96A-27E0-7F76-B371-57E854A1675A | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401b | MyBuilding.Floor24.Room2401.Table2401b | table #2401 b | 2026-09-30 03:42:13.7318341 | 2026-09-30 03:42:13.7318341 | <null>                      |
    | 01A1A96A-4DD2-73E7-8851-A9D5079B1E45 | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401c | MyBuilding.Floor24.Room2401.Table2401c | table #2401 c | 2026-09-30 03:42:16.7083398 | 2026-09-30 03:42:16.7083398 | <null>                      |
    | 01A1A96A-82B1-75B4-BD87-9894CE2E36B8 | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2402   | MyBuilding.Floor24.Room2402            | room #2402    | 2026-09-30 03:43:00.2007052 | 2026-09-30 03:43:00.2007052 | <null>                      |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2403   | MyBuilding.Floor24.Room2403            | room #2403    | 2026-09-30 03:43:19.7272716 | 2026-09-30 03:43:19.7272716 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | TRUE | Table2403a | MyBuilding.Floor24.Room2403.Table2403a | table #2403 a | 2026-09-30 03:43:49.1854194 | 2026-09-30 03:43:49.1854194 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | TRUE | Table2403b | MyBuilding.Floor24.Room2403.Table2403b | table #2403 b | 2026-09-30 03:43:52.2221069 | 2026-09-30 03:43:52.2221069 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96B-68FE-77FE-9BD8-C4C03814C7EF | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2404   | MyBuilding.Floor24.Room2404            | room #2404    | 2026-09-30 03:44:24.5148005 | 2026-09-30 03:44:24.5148005 | <null>                      |
    | 01A1A96B-7D1E-7EFB-88F2-06ADE45F5888 | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2405   | MyBuilding.Floor24.Room2405            | room #2405    | 2026-09-30 03:44:33.9484467 | 2026-09-30 03:44:33.9484467 | <null>                      |
    | 01A1A96B-9FAF-76FB-9661-4360CEB07A73 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor25    | MyBuilding.Floor25                     | floor #25     | 2026-09-30 03:45:23.2836617 | 2026-09-30 03:45:23.2836617 | 2026-10-09 12:45:51.4011112 |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor26    | MyBuilding.Floor26                     | floor #26     | 2026-09-30 03:45:45.9977765 | 2026-09-30 03:45:45.9977765 | <null>                      |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | 01A1A96C-0709-7A4E-831F-FB154377E39D | TRUE | Room2601   | MyBuilding.Floor26.Room2601            | room #2601    | 2026-09-30 03:46:10.4785162 | 2026-09-30 03:46:10.4785162 | <null>                      |
    | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | 01A1A96C-0709-7A4E-831F-FB154377E39D | TRUE | Room2602   | MyBuilding.Floor26.Room2602            | room #2602    | 2026-09-30 03:46:17.4000942 | 2026-09-30 03:46:17.4000942 | <null>                      |
    | 01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602a | MyBuilding.Floor26.Room2602.Table2602a | table #2602 a | 2026-09-30 03:46:38.2760252 | 2026-09-30 03:46:38.2760252 | <null>                      |
    | 01A1A96C-F21A-7D7F-B77B-79BB617C30ED | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602b | MyBuilding.Floor26.Room2602.Table2602b | table #2602 b | 2026-09-30 03:46:44.7748539 | 2026-09-30 03:46:44.7748539 | <null>                      |
    | 01A1A96D-000F-7AAA-9517-0E9A9C6528E7 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602c | MyBuilding.Floor26.Room2602.Table2602c | table #2602 c | 2026-10-17 20:36:30.3876071 | 2026-10-17 20:36:30.3876071 | <null>                      |
    | 01A1A96D-3A5D-7188-B085-19561F94D850 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602d | MyBuilding.Floor26.Room2602.Table2602d | table #2602 d | 2026-10-17 20:36:30.3876072 | 2026-10-17 20:36:30.3876072 | <null>                      |

Scenario Outline: 4. Get Flag by incorrect name

    When I retrieve a flag by '<Name>' and <ParentId>
    Then null is returned

Examples:
    | Name         | ParentId                             |
    | iNValID nAMe | 12345678-90AB-CDEF-1234-567890ABCDEF |
    | MyBuilding   | A554F8FA-0DE8-4899-B6BE-1CA92FFE1049 |
    | Table2602d   | CA81F335-6C3D-4A6F-9E98-6AA998509EB7 |

Scenario Outline: 5. Get Flag by Key

    When I retrieve a flag by '<Key>'
    Then the following flag is returned correctly
        | Id   | ParentId   | IsOn   | Name   | Key   | Description   | ConcurrencyToken   | CreatedAt   | UpdatedAt   | DeletedAt   |
        | <Id> | <ParentId> | <IsOn> | <Name> | <Key> | <Description> | <ConcurrencyToken> | <CreatedAt> | <UpdatedAt> | <DeletedAt> |

Examples:
    | Id                                   | ParentId                             | IsOn | Name       | Key                                    | Description   | CreatedAt                   | UpdatedAt                   | DeletedAt                   |
    | 01A1A968-05EA-7445-9702-95FFA353EA91 | <null>                               | TRUE | MyBuilding | MyBuilding                             | my building   | 2026-09-30 03:37:26.0661723 | 2026-09-30 03:37:26.0661723 | <null>                      |
    | 01A1A968-66A6-7189-A0DD-95C844F16129 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor23    | MyBuilding.Floor23                     | floor #23     | 2026-09-30 03:38:26.5139868 | 2026-09-30 03:38:26.5139868 | 2026-10-07 14:25:27.1864023 |
    | 01A1A968-9C22-77AF-9C71-14455B6B3F66 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2301   | MyBuilding.Floor23.Room2301            | room #2301    | 2026-09-30 03:38:50.2655434 | 2026-09-30 03:38:50.2655434 | 2026-10-07 14:25:27.1864023 |
    | 01A1A968-CB3C-7BBB-96D1-537EFB7333A9 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2302   | MyBuilding.Floor23.Room2302            | room #2302    | 2026-09-30 03:38:58.7826112 | 2026-09-30 03:38:58.7826112 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-105B-7F0E-9AB4-19E973672346 | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2303   | MyBuilding.Floor23.Room2303            | room #2303    | 2026-09-30 03:39:01.7470543 | 2026-09-30 03:39:01.7470543 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-5107-7075-A5F9-9ED622B2AAEB | 01A1A968-66A6-7189-A0DD-95C844F16129 | TRUE | Room2304   | MyBuilding.Floor23.Room2304            | room #2304    | 2026-09-30 03:39:04.4400251 | 2026-09-30 03:39:04.4400251 | 2026-10-07 14:25:27.1864023 |
    | 01A1A969-721E-713C-B658-1782772551C2 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor24    | MyBuilding.Floor24                     | floor #24     | 2026-09-30 03:41:02.8863859 | 2026-09-30 03:41:02.8863859 | <null>                      |
    | 01A1A969-B33C-791C-8E3B-F770A0227ADA | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2401   | MyBuilding.Floor24.Room2401            | Room #2401    | 2026-09-30 03:41:32.8614085 | 2026-09-30 03:41:32.8614085 | <null>                      |
    | 01A1A96A-109B-7B54-976C-324E4484F7A1 | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401a | MyBuilding.Floor24.Room2401.Table2401a | table #2401 a | 2026-09-30 03:42:10.6716511 | 2026-09-30 03:42:10.6716511 | <null>                      |
    | 01A1A96A-27E0-7F76-B371-57E854A1675A | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401b | MyBuilding.Floor24.Room2401.Table2401b | table #2401 b | 2026-09-30 03:42:13.7318341 | 2026-09-30 03:42:13.7318341 | <null>                      |
    | 01A1A96A-4DD2-73E7-8851-A9D5079B1E45 | 01A1A969-B33C-791C-8E3B-F770A0227ADA | TRUE | Table2401c | MyBuilding.Floor24.Room2401.Table2401c | table #2401 c | 2026-09-30 03:42:16.7083398 | 2026-09-30 03:42:16.7083398 | <null>                      |
    | 01A1A96A-82B1-75B4-BD87-9894CE2E36B8 | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2402   | MyBuilding.Floor24.Room2402            | room #2402    | 2026-09-30 03:43:00.2007052 | 2026-09-30 03:43:00.2007052 | <null>                      |
    | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2403   | MyBuilding.Floor24.Room2403            | room #2403    | 2026-09-30 03:43:19.7272716 | 2026-09-30 03:43:19.7272716 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | TRUE | Table2403a | MyBuilding.Floor24.Room2403.Table2403a | table #2403 a | 2026-09-30 03:43:49.1854194 | 2026-09-30 03:43:49.1854194 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2 | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA | TRUE | Table2403b | MyBuilding.Floor24.Room2403.Table2403b | table #2403 b | 2026-09-30 03:43:52.2221069 | 2026-09-30 03:43:52.2221069 | 2026-10-08 04:17:29.8963212 |
    | 01A1A96B-68FE-77FE-9BD8-C4C03814C7EF | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2404   | MyBuilding.Floor24.Room2404            | room #2404    | 2026-09-30 03:44:24.5148005 | 2026-09-30 03:44:24.5148005 | <null>                      |
    | 01A1A96B-7D1E-7EFB-88F2-06ADE45F5888 | 01A1A969-721E-713C-B658-1782772551C2 | TRUE | Room2405   | MyBuilding.Floor24.Room2405            | room #2405    | 2026-09-30 03:44:33.9484467 | 2026-09-30 03:44:33.9484467 | <null>                      |
    | 01A1A96B-9FAF-76FB-9661-4360CEB07A73 | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor25    | MyBuilding.Floor25                     | floor #25     | 2026-09-30 03:45:23.2836617 | 2026-09-30 03:45:23.2836617 | 2026-10-09 12:45:51.4011112 |
    | 01A1A96C-0709-7A4E-831F-FB154377E39D | 01A1A968-05EA-7445-9702-95FFA353EA91 | TRUE | Floor26    | MyBuilding.Floor26                     | floor #26     | 2026-09-30 03:45:45.9977765 | 2026-09-30 03:45:45.9977765 | <null>                      |
    | 01A1A96C-2145-7840-88C7-A03F45928635 | 01A1A96C-0709-7A4E-831F-FB154377E39D | TRUE | Room2601   | MyBuilding.Floor26.Room2601            | room #2601    | 2026-09-30 03:46:10.4785162 | 2026-09-30 03:46:10.4785162 | <null>                      |
    | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | 01A1A96C-0709-7A4E-831F-FB154377E39D | TRUE | Room2602   | MyBuilding.Floor26.Room2602            | room #2602    | 2026-09-30 03:46:17.4000942 | 2026-09-30 03:46:17.4000942 | <null>                      |
    | 01A1A96C-B820-7EB4-BE78-ECA52CAA1ED9 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602a | MyBuilding.Floor26.Room2602.Table2602a | table #2602 a | 2026-09-30 03:46:38.2760252 | 2026-09-30 03:46:38.2760252 | <null>                      |
    | 01A1A96C-F21A-7D7F-B77B-79BB617C30ED | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602b | MyBuilding.Floor26.Room2602.Table2602b | table #2602 b | 2026-09-30 03:46:44.7748539 | 2026-09-30 03:46:44.7748539 | <null>                      |
    | 01A1A96D-000F-7AAA-9517-0E9A9C6528E7 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602c | MyBuilding.Floor26.Room2602.Table2602c | table #2602 c | 2026-10-17 20:36:30.3876071 | 2026-10-17 20:36:30.3876071 | <null>                      |
    | 01A1A96D-3A5D-7188-B085-19561F94D850 | 01A1A96C-86DD-74B3-BF66-7641B575DA9B | TRUE | Table2602d | MyBuilding.Floor26.Room2602.Table2602d | table #2602 d | 2026-10-17 20:36:30.3876072 | 2026-10-17 20:36:30.3876072 | <null>                      |

Scenario Outline: 6. Get Flag by incorrect Key

    When I retrieve a flag by '<Key>'
    Then null is returned

Examples:
    | Key          |
    | iNValID nAMe |
    | MyBuilding.2 |
    | Table2602d   |

Scenario: 7. Get all deleted flags

    When I retrieve all deleted flags
    Then the following flags are returned
        | Id                                   |
        | 01A1A968-66A6-7189-A0DD-95C844F16129 |
        | 01A1A968-9C22-77AF-9C71-14455B6B3F66 |
        | 01A1A968-CB3C-7BBB-96D1-537EFB7333A9 |
        | 01A1A969-105B-7F0E-9AB4-19E973672346 |
        | 01A1A969-5107-7075-A5F9-9ED622B2AAEB |
        | 01A1A96A-8CE3-727B-8E5A-83A132AD39AA |
        | 01A1A96A-C1CA-72B2-8BE0-3E8B96A5BC90 |
        | 01A1A96B-351F-7D60-BABF-8EE3F9D6AEE2 |
        | 01A1A96B-9FAF-76FB-9661-4360CEB07A73 |
