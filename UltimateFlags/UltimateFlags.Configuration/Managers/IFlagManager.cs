using UltimateFlags.Abstraction.Entities;

namespace UltimateFlags.Configuration.Managers;

internal interface IFlagManager
{
    public Flag? Read(Guid id, bool? deleted = false);

    public Flag? Read(string name, Guid? parentId);

    public bool IsOn(string key);
}
