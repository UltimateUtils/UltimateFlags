using UltimateFlags.Abstraction.Entities;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Configuration.Managers;

internal interface IFlagManager
{
    public Flag? Read(Guid id, bool? deleted = false);

    public Flag? Read(string name, Guid? parentId);

    public Flag? Read(string key);

    public IPagedList<Flag> GetAllDeleted(int pageNumber, int pageSize);

    public bool IsOn(string key);
}
