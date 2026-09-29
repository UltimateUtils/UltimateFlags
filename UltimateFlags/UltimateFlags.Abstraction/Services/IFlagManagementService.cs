using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Abstraction.Services;

public interface IFlagManagementService
{
    /// <summary>
    ///     Creates a FLAG.
    /// </summary>
    /// <param name="creationRequest">Request contract of FLAG to create by Name and ParentId</param>
    /// <returns>Response contract of created FLAG</returns>
    /// <exception cref="FlagDuplicateFound">
    ///     FlagDuplicateFound will be thrown when the FLAG with the same KEY exists.
    /// </exception>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG specified by the ParentId does not exist.
    /// </exception>
    public FlagResponse Create(FlagCreationRequest creationRequest);

    /// <summary>
    ///     Retrieves a FLAG by the ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Response contract if found / null if not found</returns>
    public FlagResponse? Get(Guid id);

    /// <summary>
    ///     Retrieves a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Response contract</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public FlagResponse GetRequired(Guid id);

    /// <summary>
    ///     Retrieves a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <returns>Response contract if found / null if not found</returns>
    public FlagResponse? Get(string name, Guid? parentId);

    /// <summary>
    ///     Retrieves a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <returns>Response contract</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public FlagResponse GetRequired(string name, Guid? parentId);

    /// <summary>
    ///     Retrieves a FLAG by KEY.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <returns>Response contract if found / null if not found</returns>
    public FlagResponse? Get(string key);

    /// <summary>
    ///     Retrieves a FLAG by KEY.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <returns>Response contract</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public FlagResponse GetRequired(string key);

    /// <summary>
    ///     Searches and retrieves FLAGs.
    /// </summary>
    /// <remarks>
    ///     When the search string is passed in,
    ///     The FLAG NAME will be searched.
    ///     When the ParentId is passed in,
    ///     the search will be only for the specified FLAG and children recursively.
    /// </remarks>
    /// <param name="searchString">Search string</param>
    /// <param name="isOn">Is ON by key</param>
    /// <param name="pageNumber">Page Number</param>
    /// <param name="pageSize">Page Size</param>
    /// <returns>Response contracts</returns>
    public IPagedList<FlagResponse> List(
        string? searchString = null,
        bool? isOn = null,
        int pageNumber = 1,
        int pageSize = 20);

    /// <summary>
    ///     Updates a FLAG by ID. Only name may be updated.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="updateRequest">Request contract of FLAG to update</param>
    /// <returns>Response contract of updated FLAG</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public FlagResponse Update(Guid id, FlagUpdateRequest updateRequest);

    /// <summary>
    ///     Updates a FLAG.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="updateRequest">Request contract of FLAG to update</param>
    /// <returns>Number of updated FLAGs. 1 if successful. 0 otherwise.</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public int ExecuteUpdate(Guid id, FlagUpdateRequest updateRequest);

    /// <summary>
    ///     Soft-Deletes a FLAG by ID and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Response contracts of deleted FLAGs</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public IEnumerable<FlagResponse> Delete(Guid id);

    /// <summary>
    ///     Soft-Deletes a FLAG and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Number of deleted FLAGs</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public int ExecuteDelete(Guid id);

    /// <summary>
    ///     Purge/Hard-Deletes a FLAG and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Contracts of purged/hard-deleted flags.</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    /// <exception cref="FlagNotDeleted">
    ///     FlagNotDeleted will be thrown when the FLAG is not soft-deleted.
    /// </exception>
    public IEnumerable<FlagResponse> Purge(Guid id);

    /// <summary>
    ///     Purge/Hard-Deletes a FLAG and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Number of purged/hard-deleted flags.</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    /// <exception cref="FlagNotDeleted">
    ///     FlagNotDeleted will be thrown when the FLAG is not soft-deleted.
    /// </exception>
    public int ExecutePurge(Guid id);

    /// <summary>
    ///     Purge/Hard-Deletes FLAGs and their descendants recursively.
    /// </summary>
    /// <param name="fromInclusive">Deleted after (inclusive) the specified time. No limit if NULL.</param>
    /// <param name="toInclusive">Deleted before (inclusive) the specified time. No limit if NULL.</param>
    /// <returns>Number of purged/hard-deleted flags.</returns>
    public int ExecutePurge(DateTime? fromInclusive = null, DateTime? toInclusive = null);

    /// <summary>
    ///     Enables a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public void Enable(Guid id);

    /// <summary>
    ///     Enables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public void Enable(string name, Guid? parentId);

    /// <summary>
    ///     Disables a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public void Disable(Guid id);

    /// <summary>
    ///     Disables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public void Disable(string name, Guid? parentId);
}
