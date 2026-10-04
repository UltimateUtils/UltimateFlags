using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ServerFaults;

public class FlagPurgeFailed : ServerFault
{
    public FlagPurgeFailed()
    {
    }

    public FlagPurgeFailed(string? message) : base(message)
    {
    }

    protected override ServerFaultReason Reason => ServerFaultReason.FlagPurgeFailed;
}
