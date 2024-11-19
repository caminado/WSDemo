using Microsoft.EntityFrameworkCore;
using WSDemo.Domain;

namespace WSDemo.RepositoryLayer;

public class FolderItemsRepository : Repository<FolderItem>, IFolderItemsRepository
{
    #region Constructor
    public FolderItemsRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    }
    #endregion

    public async Task<IEnumerable<FolderItem>> GetByParentIdAsync(Guid? parentId, int? pageSize, int? pageNum)
    {
        IQueryable<FolderItem> qryResult;

        if (!parentId.HasValue || parentId.HasValue && parentId == Guid.Empty)
        {
            qryResult = _applicationDbContext.FolderItems.Where(x => !x.ParentId.HasValue);
        }
        else
        {
            qryResult = _applicationDbContext.FolderItems.Where(x => x.ParentId == parentId);
        }
        if (pageNum.HasValue && pageSize.HasValue)
        {
            qryResult = qryResult
                .Skip(pageNum.Value * pageSize.Value)
                .Take(pageSize.Value);
        }
        return await qryResult
            .ToListAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<FolderItem> itemsToDelete)
    {
        _applicationDbContext.FolderItems.RemoveRange(itemsToDelete);
        await _applicationDbContext.SaveChangesAsync();
    }
}
