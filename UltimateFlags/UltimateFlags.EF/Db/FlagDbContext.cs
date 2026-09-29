using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.ChangeTracking;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.EF.Db.EntityTypeConfigurations;

namespace UltimateFlags.EF.Db;

public class FlagDbContext : FlagDbContext<FlagDbContext>
{
    public FlagDbContext(DbContextOptions options)
        : base(options)
    {
    }
}

public class FlagDbContext<TContext> : DbContext, IFlagDbContext
    where TContext : DbContext, IFlagDbContext
{
    protected const string DeletedAt = "DeletedAt";

    public FlagDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Flag> Flags => Set<Flag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FlagConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    // public override int SaveChanges()
    // {
    //     _DeleteSoftly();
    //
    //     return base.SaveChanges();
    // }

    // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    // {
    //     _DeleteSoftly();
    //
    //     return await base.SaveChangesAsync(cancellationToken);
    // }

    // private void _DeleteSoftly()
    // {
    //     ChangeTracker.DetectChanges();
    //
    //     IEnumerable<EntityEntry<Flag>> deletedFlags =
    //         ChangeTracker
    //             .Entries<Flag>()
    //             .Where(e => e.State == EntityState.Deleted);
    //
    //     DateTime utcNow = DateTime.UtcNow;
    //
    //     foreach (EntityEntry<Flag> item in deletedFlags)
    //     {
    //         item.State = EntityState.Modified;
    //         item.CurrentValues[DeletedAt] = utcNow;
    //     }
    // }
}
