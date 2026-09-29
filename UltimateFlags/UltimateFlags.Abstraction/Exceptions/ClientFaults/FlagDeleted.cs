using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagDeleted : ClientFault
{
    protected override ClientFaultReason Reason => ClientFaultReason.FlagDeleted;
}
