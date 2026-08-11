using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Cart?> GetCartByCustomerIdAsync(int customerId);
    Task AddOrderAsync(Order order);
    Task AddOrderItemAsync(OrderItem orderItem);
    Task<Product?> GetProductByIdAsync(int productId);
    Task SaveChangesAsync();
    Task UpdateProductAsync(Product product);
    Task UpdateOrderAsync(Order order);
    Task ClearCartAsync(Cart cart);
    Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
    Task<List<OrderItem>> GetOrderItemsBySellerIdAsync(int sellerId);
    Task<Order?> GetByIdAsync(int orderId);
}