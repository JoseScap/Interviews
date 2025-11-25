using Core.Domain.Entities;

namespace Core.Application.Ports.Driven;

public interface IProductRepository
{
    Task<Product> SaveAsync(Product entity);
    Task<List<Product>> ListAllAsync();
    Task<Product?> ListByIdAsync(Guid id);
    Task<Product> UpdateAsync(Product entity);
    Task DeleteAsync(Product entity);
}
