using Microsoft.EntityFrameworkCore;
using UltimateFlags.EF.Db;

namespace UltimateFlags.Api.v10.Db;

public class MyFlagDbContext : FlagDbContext
{
    public MyFlagDbContext(DbContextOptions options) : base(options)
    {
    }
}
