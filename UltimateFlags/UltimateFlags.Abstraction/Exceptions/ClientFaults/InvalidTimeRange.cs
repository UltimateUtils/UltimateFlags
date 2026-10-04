using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class InvalidTimeRange : ClientFault
{
    public InvalidTimeRange()
    {
    }

    public InvalidTimeRange(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.InvalidTimeRange;
}
