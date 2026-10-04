using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class InvalidFlagName : ClientFault
{
    public InvalidFlagName()
    {
    }

    public InvalidFlagName(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.InvalidFlagName;
}
