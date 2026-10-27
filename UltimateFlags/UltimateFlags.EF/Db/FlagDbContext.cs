using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    // protected const string DeletedAt = "DeletedAt";

    protected const string ConcurrencyToken = "ConcurrencyToken";

    public FlagDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Flag> Flags => Set<Flag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FlagConfiguration());

        modelBuilder
            .Entity<Flag>()
            .Property(p => p.ConcurrencyToken)
            .IsConcurrencyToken();

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        // _DeleteSoftly();
        // _UpdateConcurrencyTokens();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // _DeleteSoftly();
        // _UpdateConcurrencyTokens();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void _UpdateConcurrencyTokens()
    {
        // 1. Get all entries that are being modified
        IEnumerable<EntityEntry<Flag>> modifiedFlags =
            ChangeTracker
                .Entries<Flag>()
                .Where(e => e.State == EntityState.Modified);

        foreach (EntityEntry<Flag> entry in modifiedFlags)
        {
            // 2. Find the ConcurrencyToken property on this entity
            PropertyEntry? tokenProperty =
                entry
                    .Properties
                    .FirstOrDefault(p => p.Metadata.IsConcurrencyToken);

            if (tokenProperty == null)
                continue;

            // 3. Check if ANY property other than the token is modified
            bool hasActualChanges =
                entry
                    .Properties
                    .Any(p => p.Metadata.Name != tokenProperty.Metadata.Name && p.IsModified); // todo - this is incorrectly true

            // 4. Only update the token if actual data changed
            if (hasActualChanges)
            {
                // Example using a Guid; replace with your token logic (e.g., DateTime.UtcNow)
                tokenProperty.CurrentValue = Guid.NewGuid();
            }
        }
    }

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
