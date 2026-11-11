# Ultimate Flags EF

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags.EF)

[Nuget](https://www.nuget.org/packages/UltimateFlags.EF)


This package implements feature flags using Entity Framework Core.


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
    Depending on how you design it, this could be the name of a feature or the name of a customer.
    Joining all the names (separated by dot, '.', character) along the flag hierarchy forms the `Key`.
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
    | MyBuilding |  <----  | Floor23 |  <----  | Room2301 |  <----  | Table2301a |
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


6. IsOn by "wrong key"

    If a key that doesn't exist is given, IsOn() returns false.
    This is a natural decision, since it asks whether the flag is on.
    For reference, there is no IsOff() method, but you can use the Exists() method to check whether a flag with the given key exists.


## Usage

There are three services in total.
```csharp
IFlagService
IFlagQueryService
IFlagCommandService
```

1. `IFlagService`

    is the most commonly used service. It has only one method, which uses a `key` to check whether the flag is on.
    `bool IsOn(string key)`


2. `IFlagQueryService`

    Provides various methods for inspecting flag values.


3. `IFlagCommandService`

    Provides various methods for changing flag values.



## DI Registration

You can register the required services in the DI container using two extension methods on `IServiceCollection`.

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

Usage examples:

```csharp
builder.Services.AddUltimateFlags<MyFlagDbContext>(
    builder.Configuration,
    options => options.UseSqlite("name=ConnectionStrings:MyFlagsDb"));

builder.Services.AddSingletonUltimateFlags<MyFlagDbContext>(
    builder.Configuration,
    options => options.UseNpgsql("name=ConnectionStrings:MyFlagsDb"));
```

1. AddUltimateFlags<TDbContext>()

    The most common way to register.
    All services are registered as `scoped`.


2. AddSingletonUltimateFlags<TDbContext>()

    All services are registered as `singleton`.
    If you need to use the UltimateFlags services in a singleton context, you must use this method.
    If you use AddUltimateFlags() instead, you will run into the [captive dependency](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines#captive-dependency) problem.
