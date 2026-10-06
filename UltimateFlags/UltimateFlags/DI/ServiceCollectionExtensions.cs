using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.Managers;
using UltimateFlags.Services;
using UltimateFlags.Storages;

namespace UltimateFlags.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUltimateFlags(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        if (configurationSection.Exists())
            services.Configure<UltimateFlagConfiguration>(configurationSection);

        services.AddScoped<IFlagQueryStorage, FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddScoped<IFlagService, FlagService>();
        services.AddScoped<IFlagQueryService, FlagQueryService>();
        services.AddScoped<IFlagCommandService, FlagCommandService>();

        return services;
    }

    public static IServiceCollection AddSingletonUltimateFlags(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        if (configurationSection.Exists())
            services.Configure<UltimateFlagConfiguration>(configurationSection);

        services.AddScoped<IFlagQueryStorage, FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddSingleton<IFlagService, Services.Singleton.FlagService>();
        services.AddSingleton<IFlagQueryService, Services.Singleton.FlagQueryService>();
        services.AddSingleton<IFlagCommandService, Services.Singleton.FlagCommandService>();

        return services;
    }

    public static IServiceCollection AddUltimateFlags<TQ, TC>(this IServiceCollection services, IConfiguration configuration)
        where TQ : class, IFlagQueryStorage
        where TC : class, IFlagCommandStorage
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        if (configurationSection.Exists())
            services.Configure<UltimateFlagConfiguration>(configurationSection);

        services.AddScoped<IFlagQueryStorage, TQ>();
        services.AddScoped<IFlagCommandStorage, TC>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddScoped<IFlagService, FlagService>();
        services.AddScoped<IFlagQueryService, FlagQueryService>();
        services.AddScoped<IFlagCommandService, FlagCommandService>();

        return services;
    }

    public static IServiceCollection AddSingletonUltimateFlags<TQ, TC>(this IServiceCollection services, IConfiguration configuration)
        where TQ : class, IFlagQueryStorage
        where TC : class, IFlagCommandStorage
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        if (configurationSection.Exists())
            services.Configure<UltimateFlagConfiguration>(configurationSection);

        services.AddScoped<IFlagQueryStorage, TQ>();
        services.AddScoped<IFlagCommandStorage, TC>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddSingleton<IFlagService, Services.Singleton.FlagService>();
        services.AddSingleton<IFlagQueryService, Services.Singleton.FlagQueryService>();
        services.AddSingleton<IFlagCommandService, Services.Singleton.FlagCommandService>();

        return services;
    }
}
