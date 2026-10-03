using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Exceptions.ClientFaults;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Abstraction.Services;

public interface IFlagQueryService
{
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
}
