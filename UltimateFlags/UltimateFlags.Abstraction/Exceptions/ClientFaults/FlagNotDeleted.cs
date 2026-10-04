using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagNotDeleted : ClientFault
{
    public FlagNotDeleted()
    {
    }

    public FlagNotDeleted(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.FlagNotDeleted;
}
