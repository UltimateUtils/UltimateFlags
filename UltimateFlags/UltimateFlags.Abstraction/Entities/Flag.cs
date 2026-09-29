namespace UltimateFlags.Abstraction.Entities;

public record Flag
{
    public Guid Id { get; init; }

    // todo - name에 들어갈 수 있는 문자 종류 제한
    public required string Name { get; init; }

    public required string Key { get; init; }

    public required bool IsOn { get; set; }

    public string? Description { get; set; }

    public required Guid? ParentId { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; set; }

    public required DateTime? DeletedAt { get; set; }

    #region navigation

    public Flag? Parent { get; init; }

    public ICollection<Flag>? Children { get; init; }

    #endregion navigation
}
