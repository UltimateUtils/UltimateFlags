using System.ComponentModel.DataAnnotations;

namespace UltimateFlags.Abstraction.Entities;

public record Flag
{
    public Guid Id { get; init; }

    public required Guid? ParentId { get; init; }

    public required bool IsOn { get; set; }

    [RegularExpression("^[^ .]*$", ErrorMessage = "Flag name may not have the following characters: dot(.), space(' ').")]
    public required string Name { get; init; }

    [RegularExpression("^[^ ]*$", ErrorMessage = "Flag key may not have the following characters: space(' ').")]
    public required string Key { get; init; }

    public string? Description { get; set; }

    public Guid ConcurrencyToken { get; set; } = Guid.NewGuid();

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; set; }

    public required DateTime? DeletedAt { get; set; }

    #region navigation

    public Flag? Parent { get; init; }

    public ICollection<Flag>? Children { get; init; }

    #endregion navigation
}
