using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Reqnroll.Assist;
using Reqnroll.Assist.ValueRetrievers;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using UltimateFlags.EF.DI;
using UltimateFlags.EF.IntegrationTests.Configs;
using UltimateFlags.EF.IntegrationTests.Db;

namespace UltimateFlags.EF.IntegrationTests.Hooks;

[Binding]
public class BasicHooks
{
    [ScenarioDependencies]
    public static IServiceCollection RegisterServices()
    {
        ServiceCollection services = new();

        IConfigurationRoot configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

        services.AddSingleton<IConfiguration>(configuration);

        IConfigurationSection configurationSection = configuration.GetRequiredSection(ServiceConfiguration.SectionName);

        ServiceConfiguration jokeGeneratorConfiguration =
            configurationSection.Get<ServiceConfiguration>()
            ?? throw new InvalidOperationException($"Configuration value of type {nameof(ServiceConfiguration)} not valid");

        services.Configure<ServiceConfiguration>(configurationSection);
        services.AddSingleton(jokeGeneratorConfiguration);

        services
            .AddLogging(
                loggingBuilder =>
                {
                    loggingBuilder.AddSeq();
                });

        string? projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
        if (projectDirectory is null)
            throw new InvalidOperationException("Invalid project directory.");

        string dbDirectory = Path.Combine(projectDirectory, "Db");
        string dbPath = Path.Combine(dbDirectory, "MyFlagsTestDb.db");

        services
            .AddUltimateFlags<MyFlagDbContext>(
                configuration,
                options => options.UseSqlite($"Data Source={dbPath}"));

        return services;
    }

    [BeforeTestRun]
    public static void RegisterNullValueRetriever()
    {
        Service.Instance.ValueRetrievers.Register(new NullValueRetriever("<null>"));
    }

    // [BeforeTestRun]
    // public static void RegisterGuidValueRetriever()
    // {
    //     Service.Instance.ValueRetrievers.Register(new GuidValueRetriever());
    // }

    [BeforeTestRun]
    public static async Task InitializeDbAsync(MyFlagDbContext flagDbContext)
    {
        _ = await flagDbContext.Database.EnsureDeletedAsync();
        await flagDbContext.Database.MigrateAsync();
    }

    [AfterTestRun]
    public static async Task CleanupDbAsync(MyFlagDbContext flagDbContext)
    {
        _ = await flagDbContext.Database.EnsureDeletedAsync();
    }
}
