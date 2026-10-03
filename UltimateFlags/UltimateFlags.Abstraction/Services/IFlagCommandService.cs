using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;

namespace UltimateFlags.Abstraction.Services;

public interface IFlagCommandService
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
