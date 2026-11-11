# Ultimate Flags Configuration

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags.Configuration)

[Nuget](https://www.nuget.org/packages/UltimateFlags.Configuration)


이것은 Configuration을 이용하여 Feature Flag을 구현한 패키지입니다.
Flag의 수가 너무 많지 않고, 자주 on/off 해야 하는 경우가 아니라면 Configuration을 이용하는 것이 가장 효율적이고 빠른 방법이 될 수 있습니다.
환경변수나 여러 외부 서비스를 이용한 Configuration을 이용하여 Flag을 생성/제어하기만 하면 `UltimateFlags` 서비스가 남은 일들을 모두 담당합니다.
Configuration에 만들어 넣을 Flag들의 형식에 대해서는 예제 [appsettings.json](https://github.com/UltimateUtils/UltimateFlags/blob/main/Examples/UltimateFlags.Configuration.Api/appsettings.json)을 참조해주세요.

 - Configuration을 변경하여 Flag을 켜고 끌 수 있기는 합니다. 밑에서 따로 설명하겠습니다.
 - Flag 개수 제한은 이론적으로 없으나, 한번에 메모리에 불러들일 수 있는 양을 초과하지 말아야 합니다.


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

    Flag의 값들을 변경하는 여러 method들이 있으나, 모두 `NotSupported` exception을 던집니다.
    Configuration의 값들을 이용하기 때문입니다. Flag 값 변경을 원하면 configuration에 직접 접근하여 변경해야 합니다.
    UltimateFlags 서비스를 이용하여 변경할 수는 없습니다. 해당 기능을 원하시면 UltimateFlags.EF 등의 다른 패키지 사용을 고려해보세요.



## DI 등록

`IServiceCollection`을 확장하는 4개의 메소드를 사용하여 필요한 서비스들을 DI 컨테이너에 등록할 수 있습니다.

[DI Registration Methods](https://github.com/UltimateUtils/UltimateFlags/blob/main/UltimateFlags/UltimateFlags.Configuration/DI/ServiceCollectionExtensions.cs)

```csharp
public static IServiceCollection AddUltimateFlags(this IServiceCollection services, IConfiguration configuration);
public static IServiceCollection AddReadOnlyUltimateFlags(this IServiceCollection services, IConfiguration configuration);
public static IServiceCollection AddSingletonUltimateFlags(this IServiceCollection services, IConfiguration configuration);
public static IServiceCollection AddSingletonReadOnlyUltimateFlags(this IServiceCollection services, IConfiguration configuration);
```

사용법 예시는 다음과 같습니다.

```csharp
builder.Services.AddUltimateFlags(builder.Configuration);
builder.Services.AddReadOnlyUltimateFlags(builder.Configuration);
builder.Services.AddSingletonUltimateFlags(builder.Configuration);
builder.Services.AddSingletonReadOnlyUltimateFlags(builder.Configuration);
```

1. AddUltimateFlags()

    가장 일반적인 등록방법입니다.
    모든 서비스들은 `scoped`로 등록됩니다.
    Configuration 변경이 반영됩니다.


2. AddReadOnlyUltimateFlags()

    모든 서비스들은 `scoped`로 등록됩니다.
    어플리케이션이 시작할 때 읽어들인 configuration 값이 계속 사용됩니다.
    Configuration을 변경하여 flag 값을 바꾸고 싶다면 어플리케이션을 재시작해야 합니다.
    하지만 flag 값들을 매번 재설정하지 않기 때문에 훨씬 효율적이고 빠릅니다.


3. AddSingletonUltimateFlags()

    모든 서비스들이 `singleton`으로 등록됩니다.
    UltimateFlags의 서비스들을 singleton 서비스에서 사용하려면 이 방법을 통하여 등록해주세요.
    하지만 일반적으로는 scoped로 등록하는 것을 권장합니다.


4. AddSingletonReadOnlyUltimateFlags()

    모든 서비스들이 `singleton`으로 등록되며, flag의 값들이 어플리케이션 시작시 읽어들인 configuration 값으로 고정됩니다.
    flag 값들의 재설정이 없어 빠르고 효율적입니다.
