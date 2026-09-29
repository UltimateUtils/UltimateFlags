using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagDuplicateFound : ClientFault
{
    protected override ClientFaultReason Reason => ClientFaultReason.FlagDuplicateFound;
}
