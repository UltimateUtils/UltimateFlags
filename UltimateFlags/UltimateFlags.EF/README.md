# Ultimate Flags EF

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags.EF)

[Nuget](https://www.nuget.org/packages/UltimateFlags.EF)


This package implements feature flags using Entity Framework Core.


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

1. AddUltimateFlags<TContext>()

    The most common way to register.
    All services are registered as `scoped`.


2. AddSingletonUltimateFlags<TContext>()

    All services are registered as `singleton`.
    If you need to use the UltimateFlags services in a singleton context, you must use this method.
    If you use AddUltimateFlags() instead, you will run into the [captive dependency](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines#captive-dependency) problem.
