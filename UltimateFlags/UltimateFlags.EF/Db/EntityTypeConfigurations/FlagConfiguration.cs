using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.EF.Db.EntityTypeConfigurations;

public class FlagConfiguration : IEntityTypeConfiguration<Flag>
{
    public void Configure(EntityTypeBuilder<Flag> flagEntity)
    {
        flagEntity.HasIndex(flag => flag.Name);
        flagEntity.HasIndex(flag => flag.IsOn);
        flagEntity.HasIndex(flag => flag.CreatedAt);
        flagEntity.HasIndex(flag => flag.UpdatedAt);
        flagEntity.HasIndex(flag => flag.DeletedAt);
        flagEntity.HasIndex(flag => new { flag.ParentId, flag.Name }).IsUnique();

        // todo - Name에 들어갈 수 있는 문자 종류 제한

        flagEntity.HasQueryFilter(flag => flag.DeletedAt == null);
    }
}
