using UltimateFlags.Abstraction.Exceptions.Reasons;

namespace UltimateFlags.Abstraction.Exceptions.ClientFaults;

public class InvalidPaginationInfo : ClientFault
{
    protected override ClientFaultReason Reason => ClientFaultReason.PaginationInfoInvalid;
}
