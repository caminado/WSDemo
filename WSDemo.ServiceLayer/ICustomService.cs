namespace WSDemo.ServiceLayer;

public interface ICustomService<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<Guid> InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id, bool withAllDescendants = false);
}
