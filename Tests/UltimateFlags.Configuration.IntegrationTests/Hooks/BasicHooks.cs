using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Reqnroll.Assist;
using Reqnroll.Assist.ValueRetrievers;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using UltimateFlags.Configuration.DI;
using UltimateFlags.Configuration.IntegrationTests.Config;

namespace UltimateFlags.Configuration.IntegrationTests.Hooks;

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

        ServiceConfiguration serviceConfiguration =
            configurationSection.Get<ServiceConfiguration>()
            ?? throw new InvalidOperationException($"Configuration value of type {nameof(ServiceConfiguration)} not valid");

        services.Configure<ServiceConfiguration>(configurationSection);
        services.AddSingleton(serviceConfiguration);

        services
            .AddLogging(
                loggingBuilder =>
                {
                    loggingBuilder.AddSeq();
                });

        services.AddUltimateFlags(configuration);

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
}
