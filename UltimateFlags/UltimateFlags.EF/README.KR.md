# Ultimate Flags EF

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags.EF)

[Nuget](https://www.nuget.org/packages/UltimateFlags.EF)


이것은 EntityFramework Core를 이용하여 Feature Flag을 구현한 패키지입니다.


## 기본 개념

UltimateFlags는 Flag들 사이의 Hierarchy를 지원합니다.
이를 활용하여 feature들 사이의 관계를 정의하고 여러 feature들을 효율적으로 관리할 수 있습니다.
Multi-Tenant 환경에서 특히 힘을 발휘할 수 있습니다.
이러한 특성으로 인해, 몇가지 특별한 개념을 이해할 필요가 있습니다.

1. `Parent`

    모든 flag은 자신의 부모 flag을 갖습니다.
    `ParentId`를 통해 연결됩니다.
    최상위 flag이라면 `ParentId` 값으로 `null`을 갖습니다.


2. `Name`과 `Key`의 차이

    각각의 flag은 자신의 `Name`을 갖습니다.
    이는 정하기에 따라, feature의 이름이 될 수도 있고, 고객의 이름이 될 수도 있습니다.
    Flag들의 hierarchy를 따라 모든 `Name`들을 dot(.) 문자로 연결하면 `Key`가 됩니다.
    `Key`는 해당 feature가 켜져 있는지 꺼져 있는지를 정할 결정적인 기준이 됩니다.
    모든 flag은 고유한 `Key` 값을 갖게 됩니다.
    일단 flag을 생성한 후에는 `Name`과 `Key` 값을 변경할 수 없습니다.


3. Soft-delete

    UltimateFlags는 soft-delete을 지원합니다.
    이는 DeletedAt 프로퍼티를 통하여 구현됩니다.

    `DateTime? DeletedAt { get; set; }`


4. Cascade Delete / Restore

    Hierarchy를 지원하는 UltimateFlags의 특성상, 특정 flag의 삭제는 해당 flag과 그 모든 자손 flag들의 삭제를 의미합니다.
    마찬가지로, 특정 flag의 restore는 해당 flag과 그 모든 조상 flag들의 restore를 의미합니다.


5. IsOn/IsOff by Key

    모든 flag은 각자의 on/off 상태를 갖습니다.
    하지만 어떤 feature가 on 상태인지 off 상태인지 체크하려면 해당 flag의 hierarchy를 살펴봐야 합니다.

    다음과 같은 flag들이 있다고 합시다.

    --------------         -----------         ------------         --------------
    | MyBuilding |         | Floor23 |         | Room2301 |         | Table2301a |
    |            | <====== |         | <====== |          | <====== |            |
    | ON         |         | OFF     |         | ON       |         | ON         |
    --------------         -----------         ------------         --------------

    Room2301과 Table2301a는 on 상태이지만, Parent인 Floor23이 off 상태이므로 다음 메소드 호출은 모두 false를 반환합니다.

```csharp
IsOn("MyBuilding.Floor23");
IsOn("MyBuilding.Floor23.Room2301");
IsOn("MyBuilding.Floor23.Room2301.Table2301a");
```

    반면 다음 메소드 호출은 true를 반환합니다.

```csharp
IsOn("MyBuilding");
```

Note:

"MyBuilding.Floor23"과 "Floor23"은 서로 다른 key임을 명심하세요.
서로 다른 flag 객체를 지칭하는 key들입니다.


6. IsOn by "non-existent key"

    존재하지 않는 key가 주어지면 IsOn()은 false를 반환합니다.
    켜져 있는지 묻는 것이기에 이는 자연스러운 결정입니다.
    참고로, IsOff() 메소드는 없지만 Exists() 메소드를 통해 해당 key를 가지고 있는 flag이 존재하는지 확인해 볼 수 있습니다.


## 사용 방법

필요한 서비스는 총 4개입니다.
```
IFlagService
IFlagQueryService
IFlagCommandService
IFlagCacheService
```

1. `IFlagService`

    가장 많이 사용될 서비스입니다. `key`를 이용하여 해당 flag이 켜져 있는지 확인하는 한 쌍의 메소드들을 가지고 있습니다.
    `bool IsOn(string key)`
    `Task<bool> IsOnAsync(string key, CancellationToken cancellationToken)`


2. `IFlagQueryService`

    Flag의 값들을 확인하는 여러 method들이 있습니다.


3. `IFlagCommandService`

    Flag의 값들을 변경하는 여러 method들이 있습니다.


4. `IFlagCacheService`

    `IFlagService`의 메소드들과 같은 결과를 반환하는 한 쌍의 메소드들이 있습니다. 단, memory cache를 통하여 결과값을 저장합니다. Cache를 통하여 가장 빠른 결과를 반환합니다.
    Cache된 값들을 제거하는 다음 두 메소드도 제공합니다.

```csharp
public void Evict(string key); // 하나의 cache 아이템 제거
public void Clear();           // cache된 모든 아이템 제거
```

    Cache의 최대 유지 시간은 configuration을 통해서 설정할 수 있습니다.
    환경변수를 이용할 경우 다음 값을 설정하면 됩니다.

    `UltimateFlags:CacheTimeLimitInSec` (Windows)
    `UltimateFlags__CacheTimeLimitInSec` (Linux)

    `appsettings.json`에 설정하는 경우 다음과 같은 모양으로 설정되어야 합니다.

```json
{
    "UltimateFlags": {
        "CacheTimeLimitInSec": 60
    }
}
```


## DI 등록

`IServiceCollection`을 확장하는 2개의 메소드를 사용하여 필요한 서비스들을 DI 컨테이너에 등록할 수 있습니다.

[DI Registration Methods](https://github.com/UltimateUtils/UltimateFlags/blob/main/UltimateFlags/UltimateFlags.EF/DI/ServiceCollectionExtensions.cs)

```csharp
public static IServiceCollection AddUltimateFlags<TContext>(
    this IServiceCollection services,
    IConfiguration configuration,
    Action<DbContextOptionsBuilder>? optionsAction);

public static IServiceCollection AddSingletonUltimateFlags<TContext>(
    this IServiceCollection services,
    IConfiguration configuration,
    Action<DbContextOptionsBuilder>? optionsAction);
```

사용법 예시는 다음과 같습니다.

```csharp
builder.Services.AddUltimateFlags<MyFlagDbContext>(
    builder.Configuration,
    options => options.UseSqlite("name=ConnectionStrings:MyFlagsDb"));

builder.Services.AddSingletonUltimateFlags<MyFlagDbContext>(
    builder.Configuration,
    options => options.UseNpgsql("name=ConnectionStrings:MyFlagsDb"));
```

1. AddUltimateFlags<TDbContext>()

    가장 일반적인 등록방법입니다.
    모든 서비스들은 `scoped`로 등록됩니다.


2. AddSingletonUltimateFlags<TDbContext>()

    모든 서비스들이 `singleton`으로 등록됩니다.
    만약 UltimateFlags의 서비스들을 singleton context에서 사용해야 한다면 이 방법을 사용해야 합니다.
    AddUltimateFlags()을 사용할 경우, [captive dependency](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines#captive-dependency) 문제를 겪게 됩니다.
