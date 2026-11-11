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

   각각의 flag은 자신의 `name`을 갖습니다.
   이는 정하기에 따라, feature의 이름이 될 수도 있고, 고객의 이름이 될 수도 있습니다.
   Flag들의 hierarchy를 따라 모든 이름을 연결하면 `Key`가 됩니다. 각 name은 '.'(dot)으로 연결합니다.
   `Key`는 해당 feature가 켜져 있는지 꺼져 있는지를 정할 결정적인 기준이 됩니다.
   모든 flag은 고유한 `Key` 값을 갖게 됩니다.
   일단 flag을 생성한 후에는 Name과 Key 값을 변경할 수 없습니다.


3. Soft-delete

   UltimateFlags는 soft-delete을 지원합니다.
   이는 DeletedAt 프로퍼티를 통하여 구현됩니다.

   `DateTime? DeletedAt { get; set; }`


4. Cascade Delete / Restore

   Hierarchy를 지원하는 UltimateFlags의 특성상, 특정 flag의 삭제는 해당 flag과 그 모든 자손 flag들의 삭제를 의미합니다.
   마찬가지로, 특정 flag의 restore는 해당 flag과 그 모든 조상 flag들의 restore를 의미합니다.



## 사용 방법

필요한 서비스는 총 3개입니다.
```csharp
IFlagService
IFlagQueryService
IFlagCommandService
```

1. `IFlagService`

   가장 많이 사용될 서비스입니다. `key`를 이용하여 해당 flag들이 켜져 있는지 확인하는 단 하나의 메소드만을 가지고 있습니다.
   `bool IsOn(string key)`


2. `IFlagQueryService`

   Flag의 값들을 확인하는 여러 method들이 있습니다.


3. `IFlagCommandService`

   Flag의 값들을 변경하는 여러 method들이 있습니다.



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
