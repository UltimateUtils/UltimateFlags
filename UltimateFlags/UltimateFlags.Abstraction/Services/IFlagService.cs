using UltimateFlags.Abstraction.Exceptions.ClientFaults;

namespace UltimateFlags.Abstraction.Services;

public interface IFlagService
{
    /// <summary>
    ///     Checks whether the specified KEY is ON or OFF. Takes all the flags in the hierarchy represented by the KEY into account.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <returns>TRUE if the specified chain of the FLAGs are all ON / FALSE otherwise</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public bool IsOn(string key);

    /// <summary>
    ///     Checks whether the specified KEY is ON or OFF. Takes all the flags in the hierarchy represented by the KEY into account.
    /// </summary>
    /// <param name="key">KEY</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>TRUE if the specified chain of the FLAGs are all ON / FALSE otherwise</returns>
    /// <exception cref="FlagNotFound">
    ///     FlagNotFound will be thrown when the FLAG with the ID does not exist.
    /// </exception>
    public Task<bool> IsOnAsync(string key, CancellationToken cancellationToken = default);
}
