using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateFlags.Abstraction.Config;
using UltimateFlags.Abstraction.Services;
using UltimateFlags.Abstraction.Storages;
using UltimateFlags.EF.Db;
using UltimateFlags.EF.Managers;
using UltimateFlags.EF.Services;
using UltimateFlags.EF.Storages;

namespace UltimateFlags.EF.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUltimateFlags<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? optionsAction)
        where TContext : FlagDbContext, IFlagDbContext
    {
        IConfigurationSection configurationSection = configuration.GetSection(UltimateFlagConfiguration.SectionName);

        if (configurationSection.Exists())
            services.Configure<UltimateFlagConfiguration>(configurationSection);

        services.AddScoped<IFlagDbContext, TContext>();
        services.AddDbContext<TContext>(optionsAction);

        services.AddScoped<IFlagQueryStorage, FlagQueryStorage>();
        services.AddScoped<IFlagCommandStorage, FlagCommandStorage>();
        services.AddScoped<IFlagManager, FlagManager>();
        services.AddScoped<IFlagService, FlagService>();
        services.AddScoped<IFlagQueryService, FlagQueryService>();
        services.AddScoped<IFlagCommandService, FlagCommandService>();

        return services;
    }
}
