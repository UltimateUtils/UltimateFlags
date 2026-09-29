namespace UltimateFlags.Abstraction.Contracts;

public record FlagCreationRequest
{
    // todo - Name에 들어갈 수 있는 문자 종류 제한
    public required string Name { get; init; }

    public required Guid? ParentId { get; init; }

    public required bool IsOn { get; init; }

    public required string? Description { get; init; }
}
