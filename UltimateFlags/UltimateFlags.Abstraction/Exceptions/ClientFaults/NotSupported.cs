using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class NotSupported : ClientFault
{
    public NotSupported()
    {
    }

    public NotSupported(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.NotSupported;
}
