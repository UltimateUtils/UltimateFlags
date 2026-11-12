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
    ///     Soft-Deletes a FLAG by ID and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="concurrencyToken">ConcurrencyToken</param>
    /// <returns>Response contracts of deleted FLAGs</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public IEnumerable<FlagResponse> Delete(Guid id, Guid? concurrencyToken = null);

    /// <summary>
    ///     Purge/Hard-Deletes a FLAG and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="concurrencyToken">ConcurrencyToken</param>
    /// <returns>Contracts of purged/hard-deleted flags.</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    /// <exception cref="FlagNotDeleted">
    ///     FlagNotDeleted will be thrown when the FLAG is not soft-deleted.
    /// </exception>
    public IEnumerable<FlagResponse> Purge(Guid id, Guid? concurrencyToken = null);

    /// <summary>
    ///     Restores deleted FLAG and its deleted ancestors
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="concurrencyToken">ConcurrencyToken</param>
    /// <returns>Restored FLAGs</returns>
    public IEnumerable<FlagResponse> Restore(Guid id, Guid? concurrencyToken = null);

    /// <summary>
    ///     Enables a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public void Enable(Guid id);

    /// <summary>
    ///     Enables a FLAG by KEY.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the KEY does not exist.
    /// </exception>
    public void Enable(string key);

    /// <summary>
    ///     Enables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the Name and ParentId does not exist.
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
    ///     Disables a FLAG by KEY.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the KEY does not exist.
    /// </exception>
    public void Disable(string key);

    /// <summary>
    ///     Disables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the Name and ParentId does not exist.
    /// </exception>
    public void Disable(string name, Guid? parentId);

    /// <summary>
    ///     Creates a FLAG.
    /// </summary>
    /// <param name="creationRequest">Request contract of FLAG to create by Name and ParentId</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Response contract of created FLAG</returns>
    /// <exception cref="FlagDuplicateFound">
    ///     FlagDuplicateFound will be thrown when the FLAG with the same KEY exists.
    /// </exception>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG specified by the ParentId does not exist.
    /// </exception>
    public Task<FlagResponse> CreateAsync(
        FlagCreationRequest creationRequest,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a FLAG by ID. Only name may be updated.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="updateRequest">Request contract of FLAG to update</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Response contract of updated FLAG</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public Task<FlagResponse> UpdateAsync(
        Guid id,
        FlagUpdateRequest updateRequest,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Soft-Deletes a FLAG by ID and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="concurrencyToken">ConcurrencyToken</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Response contracts of deleted FLAGs</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public Task<IEnumerable<FlagResponse>> DeleteAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Purge/Hard-Deletes a FLAG and its descendants recursively.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="concurrencyToken">ConcurrencyToken</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Contracts of purged/hard-deleted flags.</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    /// <exception cref="FlagNotDeleted">
    ///     FlagNotDeleted will be thrown when the FLAG is not soft-deleted.
    /// </exception>
    public Task<IEnumerable<FlagResponse>> PurgeAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Restores deleted FLAG and its deleted ancestors
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="concurrencyToken">ConcurrencyToken</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Restored FLAGs</returns>
    public Task<IEnumerable<FlagResponse>> RestoreAsync(
        Guid id,
        Guid? concurrencyToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Enables a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public Task EnableAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Enables a FLAG by KEY.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the KEY does not exist.
    /// </exception>
    public Task EnableAsync(
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Enables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the Name and ParentId does not exist.
    /// </exception>
    public Task EnableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Disables a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public Task DisableAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Disables a FLAG by KEY.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the KEY does not exist.
    /// </exception>
    public Task DisableAsync(
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Disables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the Name and ParentId does not exist.
    /// </exception>
    public Task DisableAsync(
        string name,
        Guid? parentId,
        CancellationToken cancellationToken = default);
}
