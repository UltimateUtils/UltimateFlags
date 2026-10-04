using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class FlagDuplicateFound : ClientFault
{
    public FlagDuplicateFound()
    {
    }

    public FlagDuplicateFound(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.FlagDuplicateFound;
}
