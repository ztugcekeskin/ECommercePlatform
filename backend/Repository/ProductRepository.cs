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
    public async Task<List<Product>> SearchAsync(string search)
    {
    return await _context.Products
        .Where(p => EF.Functions.ILike(p.Name, $"%{search}%"))
        .ToListAsync();
    }

        public async Task<List<Product>> SearchBySellerAsync(
    int sellerId,
    string search)
{
    return await _context.Products
        .Where(p =>
            p.SellerId == sellerId &&
            EF.Functions.ILike(p.Name, $"%{search}%"))
        .ToListAsync();
}

    public async Task<List<Product>> GetByCategoryAsync(string category)
{
    return await _context.Products
        .Where(p => p.Category == category)
        .ToListAsync();
}
    public async Task<List<Product>> FilterAsync(
    string? search,
    string? category,
    string? sort)
    {
    var query = _context.Products.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(p =>
            EF.Functions.ILike(p.Name, $"%{search}%"));
    }

    if (!string.IsNullOrWhiteSpace(category))
    {
        query = query.Where(p =>
            p.Category == category);
    }

    switch (sort)
    {
        case "price_asc":
            query = query.OrderBy(p => p.Price);
            break;

        case "price_desc":
            query = query.OrderByDescending(p => p.Price);
            break;

        case "name_asc":
            query = query.OrderBy(p => p.Name);
            break;

        case "name_desc":
            query = query.OrderByDescending(p => p.Name);
            break;
    }

    return await query.ToListAsync();
    }
    public async Task<Product?> GetProductWithSellerAsync(int id)
    {
    return await _context.Products
        .Include(p => p.Seller)
        .FirstOrDefaultAsync(p => p.Id == id);
    }

}