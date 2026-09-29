namespace UltimateFlags.Abstraction.Exceptions.Reasons;

public enum ClientFaultReason
{
    FlagNotFound,

    FlagDuplicateFound,

    FlagDeleted,

    FlagNotDeleted,

    FlagParentNotFound,

    PaginationInfoInvalid,

    InvalidTimeRange,
}
