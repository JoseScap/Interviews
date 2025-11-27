namespace Core.Application.Ports.Driven;

public interface IBaseRepository<T>
{
    Task<T> SaveAsync(T entity);
    Task<List<T>> ListAllAsync();
    Task<T?> ListByIdAsync(Guid id);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}