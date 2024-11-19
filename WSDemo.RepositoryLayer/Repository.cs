using Microsoft.EntityFrameworkCore;
using WSDemo.Domain;

namespace WSDemo.RepositoryLayer;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    #region property
    protected readonly ApplicationDbContext _applicationDbContext;
    protected DbSet<T> _entitiesProxy;
    #endregion

    #region Constructor
    public Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _entitiesProxy = _applicationDbContext.Set<T>();
    }
    #endregion

    public async Task<T?> GetByIdAsync(Guid Id)
    {
        return await _entitiesProxy.SingleOrDefaultAsync(c => c.Id == Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }
        _entitiesProxy.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task<Guid> InsertAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }
        _entitiesProxy.Add(entity);
        await SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }
        _entitiesProxy.Update(entity);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _applicationDbContext.SaveChangesAsync();
    }
}
