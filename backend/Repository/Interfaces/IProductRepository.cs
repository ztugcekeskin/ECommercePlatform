using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task SaveChangesAsync();

    Task<List<Product>> GetBySellerIdAsync(int sellerId);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> SearchAsync(string search);
    Task<List<Product>> SearchBySellerAsync(int sellerId, string search);
    Task<List<Product>> GetByCategoryAsync(string category);
    Task<List<Product>> FilterAsync(string? search, string? category, string? sort);

    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetProductWithSellerAsync(int id);
    
    Task Delete(Product product);
    Task Update(Product product);
}