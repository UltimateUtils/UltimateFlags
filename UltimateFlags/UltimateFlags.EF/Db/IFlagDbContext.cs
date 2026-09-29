using Microsoft.EntityFrameworkCore;
using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.EF.Db;

public interface IFlagDbContext : IDisposable
{
    public DbSet<Flag> Flags { get; }

    public int SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
