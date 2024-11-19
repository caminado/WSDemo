using WSDemo.Domain;

namespace WSDemo.ServiceLayer;

public class FolderItemsService : CustomService, IFolderItemsService
{
    public FolderItemsService(IFolderItemsRepository repository)
        : base(repository)
    {

    }

    public async Task<IEnumerable<FolderItem>> GetByParentIdAsync(Guid? parentId, int pageSize, int pageNum)
    {
        return await _folderItemsRepository.GetByParentIdAsync(parentId, pageSize, pageNum);
    }
}
