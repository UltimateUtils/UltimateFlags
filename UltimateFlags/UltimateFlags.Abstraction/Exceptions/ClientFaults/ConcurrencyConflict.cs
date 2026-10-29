using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class ConcurrencyConflict : ClientFault
{
    public ConcurrencyConflict()
    {
    }

    public ConcurrencyConflict(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.ConcurrencyConflict;
}
