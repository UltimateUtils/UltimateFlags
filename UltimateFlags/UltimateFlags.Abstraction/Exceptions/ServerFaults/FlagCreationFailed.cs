using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagCreationFailed : ServerFault
{
    protected override ServerFaultReason Reason => ServerFaultReason.FlagCreationFailed;
}
