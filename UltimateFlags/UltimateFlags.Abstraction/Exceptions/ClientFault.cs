using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions;

public abstract class ClientFault : UltimateFlagsExceptionBase
{
    protected ClientFault()
    {
    }

    protected ClientFault(string? message) : base(message)
    {
    }

    protected abstract ClientFaultReason Reason { get; }

    public override string GetReason()
    {
        return Reason.ToString();
    }
}
