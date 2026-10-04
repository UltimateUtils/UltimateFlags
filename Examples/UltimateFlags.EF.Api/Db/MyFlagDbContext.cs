using Microsoft.EntityFrameworkCore;
using UltimateFlags.EF.Db;

namespace UltimateFlags.EF.Api.Db;

public class MyFlagDbContext : FlagDbContext
{
    public MyFlagDbContext(DbContextOptions options) : base(options)
    {
    }
}
