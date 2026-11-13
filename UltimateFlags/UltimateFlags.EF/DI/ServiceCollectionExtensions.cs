using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.EF.Db;
using UltimateFlags.EF.Managers;
using UltimateFlags.EF.Services;
using UltimateFlags.EF.Storages;
using UltimateFlags.Utils;

namespace UltimateFlags.EF.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUltimateFlags<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? optionsAction)
        where TContext : FlagDbContext
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        if (configurationSection.Exists())
            services.Configure<UltimateFlagConfiguration>(configurationSection);

        services.AddScoped<FlagDbContext, TContext>();
        services.AddDbContext<TContext>(optionsAction);

        // services.AddMemoryCache();
        services.AddSingleton<FlagCache>();

        services.AddScoped<IFlagQueryStorage, FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddScoped<IFlagService, FlagService>();
        services.AddScoped<IFlagCacheService, FlagCacheService>();
        services.AddScoped<IFlagQueryService, FlagQueryService>();
        services.AddScoped<IFlagCommandService, FlagCommandService>();

        return services;
    }

    public static IServiceCollection AddSingletonUltimateFlags<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? optionsAction)
        where TContext : FlagDbContext
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        if (configurationSection.Exists())
            services.Configure<UltimateFlagConfiguration>(configurationSection);

        services.AddScoped<FlagDbContext, TContext>();
        services.AddDbContext<TContext>(optionsAction);

        // services.AddMemoryCache();
        services.AddSingleton<FlagCache>();

        services.AddScoped<IFlagQueryStorage, FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddSingleton<IFlagService, Services.Singleton.FlagService>();
        services.AddSingleton<IFlagCacheService, Services.Singleton.FlagCacheService>();
        services.AddSingleton<IFlagQueryService, Services.Singleton.FlagQueryService>();
        services.AddSingleton<IFlagCommandService, Services.Singleton.FlagCommandService>();

        return services;
    }
}
