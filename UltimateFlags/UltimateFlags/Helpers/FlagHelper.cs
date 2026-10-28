using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Helpers;

public static class FlagHelper
{
    public static Flag UpdatedFrom(this Flag entity, FlagUpdateRequest updateRequest)
    {
        bool updated = false;

        if (updateRequest.Description is not null)
        {
            string? description =
                updateRequest.Description == string.Empty
                    ? null
                    : updateRequest.Description;

            if (entity.Description != description)
            {
                updated = true;
                entity.Description = description;
            }
        }

        if (updateRequest.IsOn.HasValue && entity.IsOn != updateRequest.IsOn.Value)
        {
            updated = true;
            entity.IsOn = updateRequest.IsOn.Value;
        }

        return
            updated
                ? entity._Modified()
                : entity;
    }

    public static Flag Enabled(this Flag entity)
    {
        if (entity.IsOn)
            return entity;

        entity.IsOn = true;

        return entity._Modified();
    }

    public static Flag Disabled(this Flag entity)
    {
        if (!entity.IsOn)
            return entity;

        entity.IsOn = false;

        return entity._Modified();
    }

    public static Flag Deleted(this Flag entity)
    {
        if (entity.DeletedAt.HasValue)
            return entity;

        entity.DeletedAt = DateTime.UtcNow;

        return entity._Modified();
    }

    public static IEnumerable<string> GetAncestorKeys(this string key, bool inclusive)
    {
        string[] names = key.Split(Utils.Constants.KeyDelimiter);

        IEnumerable<string> x =
            inclusive
                ? names
                : names.Take(names.Length - 1);

        return x.Select((_, index) => string.Join(Utils.Constants.KeyDelimiter, names[..(index + 1)]));
    }

    private static Flag _Modified(this Flag entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        entity.ConcurrencyToken = Guid.NewGuid();

        return entity;
    }
}
