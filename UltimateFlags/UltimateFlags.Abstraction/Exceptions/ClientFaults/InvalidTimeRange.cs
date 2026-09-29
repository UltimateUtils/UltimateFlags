using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class InvalidTimeRange : ClientFault
{
    protected override ClientFaultReason Reason => ClientFaultReason.InvalidTimeRange;
}
