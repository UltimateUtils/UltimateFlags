using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagParentNotFound : ClientFault
{
    public FlagParentNotFound()
    {
    }

    public FlagParentNotFound(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.FlagParentNotFound;
}
