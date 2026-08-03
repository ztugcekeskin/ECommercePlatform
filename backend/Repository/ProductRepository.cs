using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Product>> GetBySellerIdAsync(int sellerId)
    {
        return await _context.Products
        .Where(p => p.SellerId == sellerId)
        .ToListAsync();
    }
    public async Task<List<Product>> GetAllAsync()
{
    return await _context.Products.ToListAsync();
}
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task Update(Product product)
    {
        _context.Products.Update(product);
    }

}