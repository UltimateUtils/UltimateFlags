using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Utils;
using UltimateUtils.Extensions;

namespace UltimateFlags.Converters;

internal static class FlagConverter
{
    internal static Flag ToEntity(this FlagCreationRequest creationRequest, string parentKey)
    {
        DateTime utcNow = DateTime.UtcNow;

        string delimiter =
            parentKey.IsNullOrEmpty()
                ? string.Empty
                : Constants.KeyDelimiter;

        return
            new Flag
            {
                Id = Guid.NewGuid(),
                Name = creationRequest.Name,
                Key = $"{parentKey}{delimiter}{creationRequest.Name}",
                IsOn = creationRequest.IsOn,
                Description =
                    creationRequest.Description == string.Empty
                        ? null
                        : creationRequest.Description,
                ParentId = creationRequest.ParentId,
                CreatedAt = utcNow,
                UpdatedAt = utcNow,
                DeletedAt = null,
            };
    }

    internal static FlagResponse ToContract(this Flag entity)
    {
        return
            new FlagResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Key = entity.Key,
                IsOn = entity.IsOn,
                Description = entity.Description,
                ParentId = entity.ParentId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                DeletedAt = entity.DeletedAt,
            };
    }

    internal static IEnumerable<FlagResponse> ToContracts(this IEnumerable<Flag> entities)
    {
        return entities.Select(ToContract);
    }
}
