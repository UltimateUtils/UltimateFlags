using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class InvalidPaginationInfo : ClientFault
{
    public InvalidPaginationInfo()
    {
    }

    public InvalidPaginationInfo(string? message) : base(message)
    {
    }

    protected override ClientFaultReason Reason => ClientFaultReason.PaginationInfoInvalid;
}
