using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagPurgeFailed : ServerFault
{
    protected override ServerFaultReason Reason => ServerFaultReason.FlagPurgeFailed;
}
