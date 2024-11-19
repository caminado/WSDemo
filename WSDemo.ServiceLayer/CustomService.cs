using WSDemo.Domain;

namespace WSDemo.ServiceLayer;

public class CustomService : ICustomService<FolderItem>
{
    protected readonly IFolderItemsRepository _folderItemsRepository;

    public CustomService(IFolderItemsRepository repository)
    {
        _folderItemsRepository = repository;
    }

    public async Task<FolderItem?> GetByIdAsync(Guid id)
    {
        return await _folderItemsRepository.GetByIdAsync(id);
    }

    public async Task<Guid> InsertAsync(FolderItem entity)
    {
        return await _folderItemsRepository.InsertAsync(entity);
    }

    public async Task UpdateAsync(FolderItem entity)
    {
        await _folderItemsRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id, bool withAllDescendants = false)
    {
        if (withAllDescendants) {
            var itemsToDelete = new List<FolderItem>();
            var queue = new Queue<FolderItem>();

            var root = await _folderItemsRepository.GetByIdAsync(id);
            var rootItems = await _folderItemsRepository.GetByParentIdAsync(id);
            foreach (var item in rootItems)
            {
                queue.Enqueue(item);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                itemsToDelete.Add(current);

                var children = await _folderItemsRepository.GetByParentIdAsync(current.Id);
                foreach (var child in children)
                {
                    queue.Enqueue(child);
                }
            }

            itemsToDelete.Add(root);
            await _folderItemsRepository.RemoveRangeAsync(itemsToDelete);
            return;
        }
        await _folderItemsRepository.DeleteAsync(id);
    }
}
