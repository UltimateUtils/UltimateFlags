using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagDeleted : ClientFault
{
    public FlagDeleted()
    {
    }

    public FlagDeleted(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.FlagDeleted;
}
