using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Abstraction.Storages;

public interface IFlagCommandStorage
{
    /// <summary>
    ///     Retrieves a FLAG with TRACKING by the ID.
    /// </summary>
    /// <remarks>
    ///     This method has TRACKING when used with EF context.
    ///     With non-EF context, this method is the same as Read().
    /// </remarks>
    /// <param name="id">ID</param>
    /// <param name="deleted">Checks both deleted and undeleted flags if null</param>
    /// <returns>FLAG entity if found / null if not found</returns>
    public Flag? Get(Guid id, bool? deleted = false);

    /// <summary>
    ///     Retrieves a FLAG with TRACKING by the NAME and the ParentId.
    /// </summary>
    /// <remarks>
    ///     This method has TRACKING when used with EF context.
    ///     With non-EF context, this method is the same as Read().
    /// </remarks>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <returns>FLAG entity if found / null if not found</returns>
    public Flag? Get(string name, Guid? parentId);

    /// <summary>
    ///     Retrieves children of the specified FLAG.
    /// </summary>
    /// <param name="parentId">ParentID</param>
    /// <param name="deleted">Checks both deleted and undeleted flags if null</param>
    /// <returns>Children of the specified FLAG</returns>
    public IEnumerable<Flag> GetAll(Guid? parentId, bool? deleted = false);

    /// <summary>
    ///     Creates a FLAG.
    /// </summary>
    /// <param name="flag">An entity of the FLAG</param>
    /// <returns>An entity of the created FLAG</returns>
    public Flag Create(Flag flag);

    /// <summary>
    ///     Updates a FLAG.
    /// </summary>
    /// <param name="flag">An entity of the FLAG</param>
    /// <returns>An entity of the updated FLAG</returns>
    public Flag Update(Flag flag);

    /// <summary>
    ///     Updates a FLAG.
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="contract">Update request contract</param>
    /// <returns>An entity of the updated FLAG</returns>
    public int ExecuteUpdate(Guid id, FlagUpdateRequest contract);

    /// <summary>
    ///     Soft-deletes a FLAG.
    /// </summary>
    /// <param name="flag">ID</param>
    /// <returns>An entity of the deleted FLAG</returns>
    public Flag Delete(Flag flag);

    /// <summary>
    ///     Soft-deletes a FLAG by ID.
    /// </summary>
    /// <param name="ids">IDs</param>
    /// <returns>Number of deleted FLAGs.</returns>
    public int ExecuteDelete(IEnumerable<Guid> ids);

    /// <summary>
    ///     Purges/Hard-deletes a FLAG.
    /// </summary>
    /// <param name="flag">FLAG</param>
    /// <returns>Entity of purged/hard-deleted flag.</returns>
    public Flag Purge(Flag flag);

    /// <summary>
    ///     Purges/Hard-deletes a FLAG by ID.
    /// </summary>
    /// <param name="ids">ID</param>
    /// <returns>Number of purged/hard-deleted flags. 1 if successful. 0 otherwise.</returns>
    public int ExecutePurge(IEnumerable<Guid> ids);

    /// <summary>
    ///     Enables a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Number of enabled flags. 1 if successful. 0 otherwise.</returns>
    public int Enable(Guid id);

    /// <summary>
    ///     Enables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <returns>Number of enabled flags. 1 if successful. 0 otherwise.</returns>
    public int Enable(string name, Guid? parentId);

    /// <summary>
    ///     Disables a FLAG by ID.
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Number of disabled flags. 1 if successful. 0 otherwise.</returns>
    public int Disable(Guid id);

    /// <summary>
    ///     Disables a FLAG by NAME and ParentId.
    /// </summary>
    /// <param name="name">NAME</param>
    /// <param name="parentId">ParentId</param>
    /// <returns>Number of disabled flags. 1 if successful. 0 otherwise.</returns>
    public int Disable(string name, Guid? parentId);

    /// <summary>
    ///     Saves changes to the database.
    /// </summary>
    /// <remarks>
    ///     If you implement for non-EF context, follow either approaches below:
    ///     1. Do nothing with this method
    ///         - Keep this implementation empty (not throwing NotImplementedException)
    ///         - Save changes to the DB immediately when you make changes in Create(), Update(), Delete(), Enable() and Disable().
    ///
    ///     2. You can make the implementation just like the DbContext.SaveChanges().
    ///         - Save DB changes with this method call.
    ///         - FlagService's Create, Update, Delete, Enable and Disable methods will call SaveChanges() at the end of their implementations.
    /// </remarks>
    /// <returns>The number of state entries written to the database</returns>
    public int SaveChanges();
}
