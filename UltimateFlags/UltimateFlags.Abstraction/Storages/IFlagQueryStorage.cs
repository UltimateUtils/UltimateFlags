using UltimateFlags.Abstraction.Entities;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Abstraction.Storages;

public interface IFlagQueryStorage
{
    /// <summary>
    ///     Retrieves a readonly FLAG with NO TRACKING by the ID.
    /// </summary>
    /// <remarks>
    ///     This method has NO TRACKING when used with EF context.
    ///     With non-EF context, this method is the same as Get().
    /// </remarks>
    /// <param name="id">ID</param>
    /// <param name="deleted">Checks both deleted and undeleted flags if null</param>
    /// <returns>FLAG entity if found / null if not found</returns>
    public Flag? Read(Guid id, bool? deleted = false);

    /// <summary>
    ///     Retrieves a readonly FLAG with NO TRACKING by the NAME and the ParentId.
    /// </summary>
    /// <remarks>
    ///     This method has No TRACKING when used with EF context.
    ///     With non-EF context, this method is the same as Get().
    /// </remarks>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <returns>FLAG entity if found / null if not found</returns>
    public Flag? Read(string name, Guid? parentId);

    /// <summary>
    ///     Retrieves children of the specified FLAG.
    /// </summary>
    /// <param name="parentId">ParentID</param>
    /// <param name="deleted">Checks both deleted and undeleted flags if null</param>
    /// <returns>Children of the specified FLAG</returns>
    public IQueryable<Flag> ReadAll(Guid? parentId, bool? deleted = false);

    /// <summary>
    ///     Retrieves deleted flags by their deleted time.
    /// </summary>
    /// <param name="fromInclusive">FROM deleted time (inclusive)</param>
    /// <param name="toInclusive">TO deleted time (inclusive)</param>
    /// <returns></returns>
    public IQueryable<Flag> ReadAllDeleted(DateTime? fromInclusive, DateTime? toInclusive);

    public IQueryable<Flag> ReadAllAncestors(Guid id);

    public IQueryable<Flag> ReadAllAncestors(string key);

    public IQueryable<Flag> ReadAllAncestors(string name, Guid? parentId);

    /// <summary>
    ///     Searches and retrieves FLAGs.
    /// </summary>
    /// <remarks>
    ///     When the search string is passed in,
    ///     The FLAG NAME will be searched.
    ///     When the ParentId is passed in,
    ///     the search will be only for the specified FLAG and children recursively.
    /// </remarks>
    /// <param name="searchString">Search String</param>
    /// <param name="isOn">Is ON by key</param>
    /// <param name="pageNumber">Page Number</param>
    /// <param name="pageSize">Page Size</param>
    /// <returns>Entities of the FLAGs with the specified   string in their NAMEs under the specified FLAG by ID</returns>
    public IPagedList<Flag> List(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize);

    /// <summary>
    ///     Checks if the specified flag exists.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="deleted">Checks both deleted and undeleted flags if null</param>
    /// <returns>TRUE if the flag exists. FALSE otherwise.</returns>
    public bool Exists(Guid id, bool? deleted = false);

    /// <summary>
    ///     Checks if the specified flag exists.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentID</param>
    /// <param name="deleted">Checks both deleted and undeleted flags if null</param>
    /// <returns>TRUE if the flag exists. FALSE otherwise.</returns>
    public bool Exists(string name, Guid? parentId, bool? deleted = false);

    /// <summary>
    ///     Checks whether the FLAG is ON or OFF independent of its parent.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>TRUE if the FLAG is ON / FALSE otherwise. Note that the result doesn't depend on the parent FLAG.</returns>
    public bool IsOn(Guid id);

    /// <summary>
    ///     Checks whether the FLAG is ON or OFF taking the hierarchy into account.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <returns>TRUE if the FLAG is ON taking the hierarchy into account / FALSE otherwise</returns>
    public bool IsOn(string name, Guid? parentId);
}
