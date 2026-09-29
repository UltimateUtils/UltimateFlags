using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagDeletionFailed : ServerFault
{
    protected override ServerFaultReason Reason => ServerFaultReason.FlagDeletionFailed;
}
