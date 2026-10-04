using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagUpdateFailed : ServerFault
{
    public FlagUpdateFailed()
    {
    }

    public FlagUpdateFailed(string? message) : base(message)
    {
    }

    protected override ServerFaultReason Reason => ServerFaultReason.FlagUpdateFailed;
}
