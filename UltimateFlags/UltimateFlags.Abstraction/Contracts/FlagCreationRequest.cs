using System.ComponentModel.DataAnnotations;

namespace UltimateFlags.Abstraction.Contracts;

public record FlagCreationRequest
{
    [RegularExpression("^[^ .]*$", ErrorMessage = "Flag name may not have the following characters: dot(.), space(' ').")]
    public required string Name { get; init; }

    public required Guid? ParentId { get; init; }

    public required bool IsOn { get; init; }

    public required string? Description { get; init; }
}
