namespace UltimateFlags.Abstraction.Contracts;

public record FlagUpdateRequest
{
    public bool? IsOn { get; init; }

    public string? Description { get; init; }
}
