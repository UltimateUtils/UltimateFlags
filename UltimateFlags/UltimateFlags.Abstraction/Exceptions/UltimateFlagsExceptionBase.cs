namespace UltimateFlags.Abstraction.Exceptions;

public abstract class UltimateFlagsExceptionBase : Exception
{
    public required string Area { get; set; }

    public IDictionary<string, string>? Details { get; set; }

    public abstract string GetReason();
}
