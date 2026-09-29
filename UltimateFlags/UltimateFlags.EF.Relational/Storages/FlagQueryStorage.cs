using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.EF.Db;

namespace UltimateFlags.EF.Relational.Storages;

public class FlagQueryStorage : EF.Storages.FlagQueryStorage
{
    private readonly IFlagDbContext _flagDbContext;

    public FlagQueryStorage(
        ILogger<FlagQueryStorage> logger,
        IFlagDbContext flagDbContext)
        : base(
            logger,
            flagDbContext)
    {
        _flagDbContext = flagDbContext;
    }

    public override IQueryable<Flag> ReadAllAncestors(Guid id)
    {
        return _flagDbContext.Flags.FromSql(
            $"""
             WITH RECURSIVE ancestor_tree AS (
                 -- 1. Anchor Member: Select the starting child record
                 SELECT Id, Name, Key, IsOn, Description, ParentId, CreatedAt, UpdatedAt, DeletedAt, 1 AS level
                 FROM Flags
                 WHERE Id = {id}

                 UNION ALL

                 -- 2. Recursive Member: Join the anchor with its parent
                 SELECT t.Id, t.Name, t.Key, t.IsOn, t.Description, t.ParentId, t.CreatedAt, t.UpdatedAt, t.DeletedAt, a.level + 1 AS level
                 FROM Flags t
                      INNER JOIN ancestor_tree a ON t.Id = a.ParentId
             )
             -- 3. Final execution
             SELECT * FROM ancestor_tree
             ORDER BY level DESC
             """);
    }

    public override IQueryable<Flag> ReadAllAncestors(string key)
    {
        IQueryable<Flag> readAllAncestors = _flagDbContext.Flags.FromSql(
            $"""
             WITH RECURSIVE ancestor_tree AS (
                 -- 1. Anchor Member: Select the starting child record
                 SELECT Id, Name, Key, IsOn, Description, ParentId, CreatedAt, UpdatedAt, DeletedAt, 1 AS level
                 FROM Flags
                 WHERE Key = {key}

                 UNION ALL

                 -- 2. Recursive Member: Join the anchor with its parent
                 SELECT t.Id, t.Name, t.Key, t.IsOn, t.Description, t.ParentId, t.CreatedAt, t.UpdatedAt, t.DeletedAt, a.level + 1 AS level
                 FROM Flags t
                      INNER JOIN ancestor_tree a ON t.Id = a.ParentId
             )
             -- 3. Final execution
             SELECT * FROM ancestor_tree
             ORDER BY level DESC
             """);

        return readAllAncestors;
    }

    public override IQueryable<Flag> ReadAllAncestors(string name, Guid? parentId)
    {
        return _flagDbContext.Flags.FromSql(
            $"""
             WITH RECURSIVE ancestor_tree AS (
                 -- 1. Anchor Member: Select the starting child record
                 SELECT Id, Name, Key, IsOn, Description, ParentId, CreatedAt, UpdatedAt, DeletedAt, 1 AS level
                 FROM Flags
                 WHERE Name = {name} AND ParentId IS {parentId}

                 UNION ALL

                 -- 2. Recursive Member: Join the anchor with its parent
                 SELECT t.Id, t.Name, t.Key, t.IsOn, t.Description, t.ParentId, t.CreatedAt, t.UpdatedAt, t.DeletedAt, a.level + 1 AS level
                 FROM Flags t
                      INNER JOIN ancestor_tree a ON t.Id = a.ParentId
             )
             -- 3. Final execution
             SELECT * FROM ancestor_tree
             ORDER BY level DESC
             """);
    }
}
