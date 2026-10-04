using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions;

public abstract class ServerFault : UltimateFlagsExceptionBase
{
    protected ServerFault()
    {
    }

    protected ServerFault(string? message) : base(message)
    {
    }

    protected abstract ServerFaultReason Reason { get; }

    public override string GetReason()
    {
        return Reason.ToString();
    }
}
