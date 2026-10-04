using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagNotFound : ClientFault
{
    public FlagNotFound()
    {
    }

    public FlagNotFound(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.FlagNotFound;
}
