using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateFlags.Abstraction.Managers;
using UltimateFlags.DI;
using UltimateFlags.EF.Db;
using UltimateFlags.EF.Relational.Managers;
using UltimateFlags.EF.Relational.Storages;

namespace UltimateFlags.EF.Relational.DI;

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
