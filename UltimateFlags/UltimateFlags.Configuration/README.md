# Ultimate Flags Configuration

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags.Configuration)

[Nuget](https://www.nuget.org/packages/UltimateFlags.Configuration)


This package implements feature flags using Configuration.
If you don't have too many flags and don't need to toggle them on/off frequently, using Configuration can be the most efficient and fastest approach.
Just create and control your flags through Configuration — environment variables or various external configuration providers — and the `UltimateFlags` services take care of the rest.
For the format of the flags to put into your Configuration, please refer to the example [appsettings.json](https://github.com/UltimateUtils/UltimateFlags/blob/main/Examples/UltimateFlags.Configuration.Api/appsettings.json).

 - You can still turn flags on and off by changing the Configuration. This is explained separately below.
 - There is theoretically no limit on the number of flags, but they must not exceed what can be loaded into memory at once.


## Core Concepts

UltimateFlags supports hierarchies between flags.
You can use this to define relationships between features and manage many features efficiently.
It is especially powerful in multi-tenant environments.
Because of this, there are a few special concepts you need to understand.

1. `Parent`

    Every flag has its own parent flag.
    They are linked through `ParentId`.
    A top-level flag has `null` as its `ParentId`.


2. The difference between `Name` and `Key`

    Each flag has its own `Name`.
    Depending on how you design it, this could be the name of a feature or the name of a customer, client, or tenant.
    Joining all the `Name`s (separated by dot, '.', character) along the flag hierarchy forms the `Key`.
    The `Key` is the definitive criterion for determining whether a feature is on or off.
    Every flag has a unique `Key`.
    Once a flag is created, its `Name` and `Key` cannot be changed.


3. Soft-delete

    UltimateFlags supports soft-delete.
    This is implemented through the `DeletedAt` property.

    `DateTime? DeletedAt { get; set; }`


4. Cascade Delete / Restore

    Because UltimateFlags supports hierarchies, deleting a flag means deleting that flag and all of its descendant flags.
    Likewise, restoring a flag means restoring that flag and all of its ancestor flags.


5. IsOn/IsOff by Key

    Every flag has its own on/off state.
    However, to check whether a feature is on or off, you need to look at the flag's hierarchy.

    Suppose you have the following flags:

    --------------         -----------         ------------         --------------
    | MyBuilding |         | Floor23 |         | Room2301 |         | Table2301a |
    |            | <====== |         | <====== |          | <====== |            |
    | ON         |         | OFF     |         | ON       |         | ON         |
    --------------         -----------         ------------         --------------

    Room2301 and Table2301a are on, but because their parent Floor23 is off, all of the following method calls return false.

```csharp
IsOn("MyBuilding.Floor23");
IsOn("MyBuilding.Floor23.Room2301");
IsOn("MyBuilding.Floor23.Room2301.Table2301a");
```

    However, the following method call returns true.

```csharp
IsOn("MyBuilding");
```

Note:

Keep in mind that "MyBuilding.Floor23" and "Floor23" are different keys.
They refer to different flag objects.


6. IsOn by "non-existent key"

    If a key that doesn't exist is given, IsOn() returns false.
    This is a natural decision, since it asks whether the flag is on.
    For reference, there is no IsOff() method, but you can use the Exists() method to check whether a flag with the given key exists.


## Usage

There are four services in total.
```
IFlagService
IFlagQueryService
IFlagCommandService
IFlagCacheService
```

1. `IFlagService`

    is the most commonly used service. It has a pair of methods, which use a `key` to check whether the flag is on.
    `bool IsOn(string key)`
    `Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default)`


2. `IFlagQueryService`

    Provides various methods for inspecting flag values.


3. `IFlagCommandService`

    Provides various methods for changing flag values, but all of them throw a `NotSupported` exception.
    This is because the values come from Configuration. To change flag values, you must access and modify the configuration directly.
    They cannot be changed through the UltimateFlags services. If you need that functionality, consider using another package such as `UltimateFlags.EF`.


4. `IFlagCacheService`

    Provides a pair of methods that return the same results as the methods of `IFlagService`, except that the results are stored in a memory cache. They return results the fastest by using the cache.
    It also provides the following two methods for removing cached values.

```csharp
public void Evict(string key); // Remove a single cache item
public void Clear();           // Remove all cached items
```

    The maximum cache duration can be set through configuration.
    When using environment variables, set the following value:

    `UltimateFlags:CacheTimeLimitInSec` (Windows)
    `UltimateFlags__CacheTimeLimitInSec` (Linux)

    When configuring it in `appsettings.json`, it should look like this:

```json
{
    "UltimateFlags": {
        "CacheTimeLimitInSec": 60
    }
}
```


## DI Registration

You can register the required services in the DI container using four extension methods on `IServiceCollection`.

[DI Registration Methods](https://github.com/UltimateUtils/UltimateFlags/blob/main/UltimateFlags/UltimateFlags.Configuration/DI/ServiceCollectionExtensions.cs)

```csharp
public static IServiceCollection AddUltimateFlags(this IServiceCollection services, IConfiguration configuration);
public static IServiceCollection AddReadOnlyUltimateFlags(this IServiceCollection services, IConfiguration configuration);
public static IServiceCollection AddSingletonUltimateFlags(this IServiceCollection services, IConfiguration configuration);
public static IServiceCollection AddSingletonReadOnlyUltimateFlags(this IServiceCollection services, IConfiguration configuration);
```

Usage examples:

```csharp
builder.Services.AddUltimateFlags(builder.Configuration);
builder.Services.AddReadOnlyUltimateFlags(builder.Configuration);
builder.Services.AddSingletonUltimateFlags(builder.Configuration);
builder.Services.AddSingletonReadOnlyUltimateFlags(builder.Configuration);
```

1. AddUltimateFlags()

    The most common way to register.
    All services are registered as `scoped`.
    Configuration changes are reflected.


2. AddReadOnlyUltimateFlags()

    All services are registered as `scoped`.
    The configuration values read at application startup continue to be used.
    If you want to change flag values by modifying the configuration, you must restart the application.
    However, since flag values are not reloaded every time, it is much more efficient and faster.


3. AddSingletonUltimateFlags()

    All services are registered as `singleton`.
    Use this method if you need to use the `UltimateFlags` services from singleton services.
    In general, however, registering them as scoped is recommended.


4. AddSingletonReadOnlyUltimateFlags()

    All services are registered as `singleton`, and flag values are fixed to the configuration values read at application startup.
    Since flag values are never reloaded, it is fast and efficient.
