using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagUpdateFailed : ServerFault
{
    protected override ServerFaultReason Reason => ServerFaultReason.FlagUpdateFailed;
}
