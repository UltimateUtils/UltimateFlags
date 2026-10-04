using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagCreationFailed : ServerFault
{
    public FlagCreationFailed()
    {
    }

    public FlagCreationFailed(string? message) : base(message)
    {
    }

    protected override ServerFaultReason Reason => ServerFaultReason.FlagCreationFailed;
}
