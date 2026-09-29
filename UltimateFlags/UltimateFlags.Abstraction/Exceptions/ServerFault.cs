using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions;

public abstract class ServerFault : UltimateFlagsExceptionBase
{
    protected abstract ServerFaultReason Reason { get; }

    public override string GetReason()
    {
        return Reason.ToString();
    }
}
