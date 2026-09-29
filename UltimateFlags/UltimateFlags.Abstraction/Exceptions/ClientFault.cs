using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions;

public abstract class ClientFault : UltimateFlagsExceptionBase
{
    protected abstract ClientFaultReason Reason { get; }

    public override string GetReason()
    {
        return Reason.ToString();
    }
}
