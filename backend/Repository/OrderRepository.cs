using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Cart?> GetCartByCustomerIdAsync(int customerId)
{
    return await _context.Carts
        .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);
}

public async Task AddOrderAsync(Order order)
{
    await _context.Orders.AddAsync(order);
}

public async Task AddOrderItemAsync(OrderItem orderItem)
{
    await _context.OrderItems.AddAsync(orderItem);
}

public async Task<Product?> GetProductByIdAsync(int productId)
{
    return await _context.Products
        .FirstOrDefaultAsync(p => p.Id == productId);
}

public async Task SaveChangesAsync()
{
    await _context.SaveChangesAsync();
}

public async Task ClearCartAsync(Cart cart)
{
    _context.CartItems.RemoveRange(cart.CartItems);

    _context.Carts.Remove(cart);

    await Task.CompletedTask;
}
public async Task UpdateProductAsync(Product product)
{
    _context.Products.Update(product);

    await Task.CompletedTask;
}

public async Task UpdateOrderAsync(Order order)
{
    _context.Orders.Update(order);

    await Task.CompletedTask;
}
public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
{
    return await _context.Orders
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
        .Where(o => o.CustomerId == customerId)
        .OrderByDescending(o => o.OrderDate)
        .ToListAsync();
}
public async Task<List<OrderItem>> GetOrderItemsBySellerIdAsync(int sellerId)
{
    return await _context.OrderItems
        .Include(oi => oi.Product)
        .Include(oi => oi.Order)
        .Where(oi => oi.Product.SellerId == sellerId)
        .OrderByDescending(oi => oi.Order.OrderDate)
        .ToListAsync();
}
}