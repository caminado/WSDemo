namespace WSDemo.Domain;

public interface IFolderItemsRepository : IRepository<FolderItem>
{
    Task<IEnumerable<FolderItem>> GetByParentIdAsync(Guid? parentId, int? pageSize = null, int? pageNum = null);
    Task RemoveRangeAsync(IEnumerable<FolderItem> itemsToDelete);
}
