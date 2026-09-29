using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagParentNotFound : ClientFault
{
    protected override ClientFaultReason Reason => ClientFaultReason.FlagParentNotFound;
}
