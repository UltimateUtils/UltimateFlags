namespace UltimateFlags.Configuration.Managers;

internal interface IFlagManager
{
    public bool IsOn(string key);
}
