using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagNotDeleted : ClientFault
{
    protected override ClientFaultReason Reason => ClientFaultReason.FlagNotDeleted;
}
