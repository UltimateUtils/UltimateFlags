# Ultimate Flags Abstraction

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags.Abstraction)

[Nuget](https://www.nuget.org/packages/UltimateFlags.Abstraction)

This is the abstraction package for the `UltimateFlags` packages.
It is not intended to be referenced directly from client code.

Please use one of the following packages depending on your use case.

## UltimateFlags packages

### UltimateFlags.Configuration

If you have a relatively small number of flags and don't need to toggle them on/off frequently, you can achieve the goals of feature flags using Configuration.
It is a very fast and effective approach, but if you toggle flags often, consider the other packages that use a database.

### UltimateFlags.EF

Use this package to manage flags with Entity Framework Core.
You can use any database supported by EF Core.
There are also plans to release separate packages that support document databases such as MongoDB.
