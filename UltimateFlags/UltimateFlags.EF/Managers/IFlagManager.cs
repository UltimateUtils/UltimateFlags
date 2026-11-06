using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimatePagination.Abstraction;

namespace UltimateFlags.EF.Managers;

internal interface IFlagManager
{
    public Flag Create(Flag entity);

    public Flag? Read(Guid id, bool? deleted = false);

    public Flag? Read(string key);

    public Flag? Read(string name, Guid? parentId);

    public IPagedList<Flag> GetAllDeleted(int pageNumber, int pageSize);

    public IPagedList<Flag> List(
        string? searchString,
        bool? isOn,
        int pageNumber,
        int pageSize);

    public Flag Update(Guid id, FlagUpdateRequest updateRequest);

    public List<Flag> Delete(Guid id, Guid? concurrencyToken = null);

    public List<Flag> Purge(Guid id, Guid? concurrencyToken = null);

    public List<Flag> Restore(Guid id, Guid? concurrencyToken = null);

    public bool Exists(Guid id, bool? deleted = false);

    public bool Exists(string name, Guid? parentId, bool? deleted = false);

    public void Enable(Guid id);

    public void Enable(string key);

    public void Enable(string name, Guid? parentId);

    public void Disable(Guid id);

    public void Disable(string key);

    public void Disable(string name, Guid? parentId);

    public bool IsOn(string key);

    public int SaveChanges();
}
