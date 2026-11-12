using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Configuration.Config;
using UltimateFlags.Configuration.Managers;
using UltimateFlags.Configuration.Services;
using UltimateFlags.Configuration.Storages;

namespace UltimateFlags.Configuration.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUltimateFlags(this IServiceCollection services, IConfiguration configuration)
    {
        services._AddUltimateFlagConfiguration(configuration);

        services.AddScoped<IFlagQueryStorage, FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddScoped<IFlagService, FlagService>();
        services.AddScoped<IFlagQueryService, FlagQueryService>();
        services.AddScoped<IFlagCommandService, FlagCommandService>();

        return services;
    }

    public static IServiceCollection AddReadOnlyUltimateFlags(this IServiceCollection services, IConfiguration configuration)
    {
        services._AddFlagsComplex(configuration);

        services.AddScoped<IFlagQueryStorage, Storages.ReadOnly.FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, Storages.ReadOnly.FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddScoped<IFlagService, FlagService>();
        services.AddScoped<IFlagQueryService, FlagQueryService>();
        services.AddScoped<IFlagCommandService, FlagCommandService>();

        return services;
    }

    public static IServiceCollection AddSingletonUltimateFlags(this IServiceCollection services, IConfiguration configuration)
    {
        services._AddUltimateFlagConfiguration(configuration);

        services.AddScoped<IFlagQueryStorage, FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddSingleton<IFlagService, Services.Singleton.FlagService>();
        services.AddSingleton<IFlagQueryService, Services.Singleton.FlagQueryService>();
        services.AddSingleton<IFlagCommandService, Services.Singleton.FlagCommandService>();

        return services;
    }

    public static IServiceCollection AddSingletonReadOnlyUltimateFlags(this IServiceCollection services, IConfiguration configuration)
    {
        services._AddFlagsComplex(configuration);

        services.AddScoped<IFlagQueryStorage, Storages.ReadOnly.FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, Storages.ReadOnly.FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddSingleton<IFlagService, Services.Singleton.FlagService>();
        services.AddSingleton<IFlagQueryService, Services.Singleton.FlagQueryService>();
        services.AddSingleton<IFlagCommandService, Services.Singleton.FlagCommandService>();

        return services;
    }

    private static IServiceCollection _AddUltimateFlagConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        return services.Configure<UltimateFlagConfiguration>(configurationSection);
    }

    private static IServiceCollection _AddFlagsComplex(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);
        UltimateFlagConfiguration? ultimateFlagConfiguration = configurationSection.Get<UltimateFlagConfiguration>();

        return
            services
                .AddSingleton(
                    ultimateFlagConfiguration is not null
                        ? ultimateFlagConfiguration.Flags.GetFlagsComplex()
                        : new FlagsComplex
                        {
                            All = [],
                            ByKey = new Dictionary<string, Flag>(),
                            ById = new Dictionary<Guid, Flag>(),
                            ByName = new Dictionary<(string, Guid?), Flag>(),
                        });
    }
}
