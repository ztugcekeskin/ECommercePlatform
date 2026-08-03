using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByCustomerIdAsync(int customerId);

    Task AddCartAsync(Cart cart);

    Task AddCartItemAsync(CartItem cartItem);

    Task<CartItem?> GetCartItemAsync(int cartId, int productId);

    Task SaveChangesAsync();
}