using Microsoft.EntityFrameworkCore;
using UltimateFlags.EF.Api.Config;
using UltimateFlags.EF.Api.Db;
using UltimateFlags.EF.Api.Services;
using UltimateFlags.EF.Api.Services.Abstraction;
using UltimateFlags.EF.DI;

namespace UltimateFlags.EF.Api.Utils;

internal static class HostingExtensions
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder
                .Services
                .AddLogging(
                    loggingBuilder =>
                    {
                        loggingBuilder.AddSeq();
                    });
        }

        // Add services to the container.
        builder.Services._AddServiceDependencies(builder.Configuration);

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder.Build();
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    private static void _AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceConfiguration>(configuration.GetRequiredSection(ServiceConfiguration.SectionName));

        services.AddTransient<IHealthCheckService, HealthCheckService>();
        services.AddUltimateFlags<MyFlagDbContext>(
            configuration,
            options => options.UseSqlite("name=ConnectionStrings:MyFlagsDb"));
    }
}
