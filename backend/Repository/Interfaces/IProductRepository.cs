using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task SaveChangesAsync();
    Task<List<Product>> GetBySellerIdAsync(int sellerId);
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task Delete(Product product);
    Task Update(Product product);
}