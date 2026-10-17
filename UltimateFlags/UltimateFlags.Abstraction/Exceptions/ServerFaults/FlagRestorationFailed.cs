using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagRestorationFailed : ServerFault
{
    public FlagRestorationFailed()
    {
    }

    public FlagRestorationFailed(string? message) : base(message)
    {
    }

    protected override ServerFaultReason Reason => ServerFaultReason.FlagRestorationFailed;
}
