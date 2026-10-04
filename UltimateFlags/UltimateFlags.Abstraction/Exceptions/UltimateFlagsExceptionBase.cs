namespace UltimateFlags.Abstraction.Exceptions;

public abstract class UltimateFlagsExceptionBase : Exception
{
    protected UltimateFlagsExceptionBase()
    {
    }

    protected UltimateFlagsExceptionBase(string? message) : base(message)
    {
    }

    public required string Area { get; set; }

    public IDictionary<string, string>? Details { get; set; }

    public abstract string GetReason();
}
