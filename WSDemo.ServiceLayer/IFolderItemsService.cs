using WSDemo.Domain;

namespace WSDemo.ServiceLayer;

public interface IFolderItemsService : ICustomService<FolderItem>
{
    Task<IEnumerable<FolderItem>> GetByParentIdAsync(Guid? parentId, int pageSize, int pageNum);
}
