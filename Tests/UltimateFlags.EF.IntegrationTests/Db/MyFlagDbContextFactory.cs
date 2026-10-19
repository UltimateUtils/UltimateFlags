using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UltimateFlags.EF.IntegrationTests.Db;

public class MyFlagDbContextFactory : IDesignTimeDbContextFactory<MyFlagDbContext>
{
    public MyFlagDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

        string? connectionString = configuration.GetConnectionString("MyFlagsDb");

        DbContextOptionsBuilder<MyFlagDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlite(connectionString);

        return new MyFlagDbContext(optionsBuilder.Options);
    }
}
