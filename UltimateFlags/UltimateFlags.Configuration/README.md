# Ultimate Flags Configuration

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags.Configuration)

[Nuget](https://www.nuget.org/packages/UltimateFlags.Configuration)


This package implements feature flags using Configuration.
If you don't have too many flags and don't need to toggle them on/off frequently, using Configuration can be the most efficient and fastest approach.
Just create and control your flags through Configuration (environment variables, various external services, etc.), and the `UltimateFlags` services take care of the rest.
For the format of the flags to put in your Configuration, please refer to the example [appsettings.json](https://github.com/UltimateUtils/UltimateFlags/blob/main/Examples/UltimateFlags.Configuration.Api/appsettings.json).

 - You can toggle flags on and off by changing the Configuration. This is explained separately below.
 - There is theoretically no limit on the number of flags, but they must not exceed the amount that can be loaded into memory at once.


## Basic Concepts

UltimateFlags supports a hierarchy between flags.
You can use this to define relationships between features and manage many features efficiently.
It is especially powerful in multi-tenant environments.
Because of this, there are a few special concepts you need to understand.

1. `Parent`

    Every flag has its own parent flag.
    They are linked through `ParentId`.
    A top-level flag has `null` as its `ParentId`.


2. The difference between `Name` and `Key`

    Each flag has its own `name`.
    Depending on how you design it, this can be the name of a feature or the name of a customer.
    Joining all names along the flag hierarchy forms the `Key`. Each name is joined with a '.' (dot).
    The `Key` is the decisive criterion for whether the feature is on or off.
    Every flag has a unique `Key`.
    Once a flag is created, its Name and Key cannot be changed.


3. Soft-delete

    UltimateFlags supports soft-delete.
    This is implemented through the `DeletedAt` property.

    `DateTime? DeletedAt { get; set; }`


4. Cascade Delete / Restore

    Because UltimateFlags supports a hierarchy, deleting a flag means deleting that flag and all of its descendant flags.
    Likewise, restoring a flag means restoring that flag and all of its ancestor flags.



## Usage

There are three services in total.
```csharp
IFlagService
IFlagQueryService
IFlagCommandService
```

1. `IFlagService`

    The most commonly used service. It has only one method, which checks whether the flag is on using its `key`.
    `bool IsOn(string key)`


2. `IFlagQueryService`

    Provides various methods for reading flag values.


3. `IFlagCommandService`

    Provides various methods for changing flag values, but all of them throw a `NotSupportedException`.
    This is because the values come from Configuration. To change a flag value, you must change the configuration directly.
    You cannot change it through the UltimateFlags services. If you need this functionality, consider using another package such as UltimateFlags.EF.



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
    The configuration values read at application startup are used throughout.
    If you want to change flag values by changing the configuration, you must restart the application.
    However, since flag values are not reloaded every time, it is much more efficient and faster.


3. AddSingletonUltimateFlags()

    All services are registered as `singleton`.
    Use this method if you need to use the UltimateFlags services from singleton services.
    In general, however, registering as scoped is recommended.


4. AddSingletonReadOnlyUltimateFlags()

    All services are registered as `singleton`, and flag values are fixed to the configuration values read at application startup.
    Since flag values are never reloaded, it is fast and efficient.
