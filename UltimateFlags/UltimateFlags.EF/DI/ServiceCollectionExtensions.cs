using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateFlags.Abstraction.Managers;
using UltimateFlags.DI;
using UltimateFlags.EF.Db;
using UltimateFlags.EF.Managers;
using UltimateFlags.EF.Storages;

namespace UltimateFlags.EF.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUltimateFlags<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? optionsAction)
        where TContext : DbContext, IFlagDbContext
    {
        services.AddScoped<IFlagDbContext, TContext>();
        services.AddDbContext<TContext>(optionsAction);
        services.AddUltimateFlags<FlagQueryStorage, FlagCommandStorage>(configuration);
        services.AddScoped<IFlagManager, FlagManager>();

        return services;
    }
}
