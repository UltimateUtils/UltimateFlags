namespace UltimateFlags.Abstraction.Contracts;

public record FlagResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Key { get; init; }

    public required bool IsOn { get; init; }

    public required string? Description { get; init; }

    public required Guid? ParentId { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; init; }

    public required DateTime? DeletedAt { get; init; }
}
