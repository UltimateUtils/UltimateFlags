using UltimateFlags.Abstraction.Contracts;
using UltimateFlags.Abstraction.Entities;
using UltimateFlags.Abstraction.Managers;
using UltimatePagination.Abstraction;

namespace UltimateFlags.Managers;

internal class FlagManager : IFlagManager
{
    public Flag Create(Flag entity)
    {
        throw new NotImplementedException();
    }

    public Flag? Read(Guid id, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public Flag? Read(string key)
    {
        throw new NotImplementedException();
    }

    public Flag? Read(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public IPagedList<Flag> List(string? searchString, bool? isOn, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Flag Update(Guid id, FlagUpdateRequest contract)
    {
        throw new NotImplementedException();
    }

    public int ExecuteUpdate(Guid id, FlagUpdateRequest contract)
    {
        throw new NotImplementedException();
    }

    public List<Flag> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public int ExecuteDelete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Flag> Purge(Guid id)
    {
        throw new NotImplementedException();
    }

    public int ExecutePurge(Guid id)
    {
        throw new NotImplementedException();
    }

    public int ExecutePurge(DateTime? fromInclusive = null, DateTime? toInclusive = null)
    {
        throw new NotImplementedException();
    }

    public bool Exists(Guid id, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public bool Exists(string name, Guid? parentId, bool? deleted)
    {
        throw new NotImplementedException();
    }

    public void Enable(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Enable(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public void Disable(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Disable(string name, Guid? parentId)
    {
        throw new NotImplementedException();
    }

    public bool IsOn(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool IsOn(string key)
    {
        throw new NotImplementedException();
    }

    public int SaveChanges()
    {
        throw new NotImplementedException();
    }
}
