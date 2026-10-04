using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagDeletionFailed : ServerFault
{
    public FlagDeletionFailed()
    {
    }

    public FlagDeletionFailed(string? message) : base(message)
    {
    }

    protected override ServerFaultReason Reason => ServerFaultReason.FlagDeletionFailed;
}
