using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        var cart = await _cartRepository.GetCartByCustomerIdAsync(dto.CustomerId);

        if (cart == null)
        {
            cart = new Cart
            {
                CustomerId = dto.CustomerId
            };

            await _cartRepository.AddCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, dto.ProductId);

        if (cartItem != null)
        {
            cartItem.Quantity += dto.Quantity;
        }
        else
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _cartRepository.AddCartItemAsync(cartItem);
        }
        await _cartRepository.SaveChangesAsync();

        return Ok(new
        {
            message = "Ürün sepete eklendi."
        });
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCart(int customerId)
    {
    var cart = await _cartRepository.GetCartWithProductsAsync(customerId);

    if (cart == null)
    {
        return Ok(new List<object>());
    }

    var result = cart.CartItems.Select(ci => new
    {
        id = ci.Id,
        quantity = ci.Quantity,

        product = new
        {
            ci.Product.Id,
            ci.Product.Name,
            ci.Product.Description,
            ci.Product.Price,
            ci.Product.Stock,
            ci.Product.ImageUrl
        }
    });

    return Ok(result);
    }
    
    [HttpPut("increase/{cartItemId}")]
    public async Task<IActionResult> IncreaseQuantity(int cartItemId)
    {
    var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

    if (cartItem == null)
    {
        return NotFound(new
        {
            message = "Ürün bulunamadı."
        });
    }

    cartItem.Quantity++;

    await _cartRepository.SaveChangesAsync();

    return Ok(new
    {
        message = "Adet artırıldı."
    });
    }

    [HttpPut("decrease/{cartItemId}")]
    public async Task<IActionResult> DecreaseQuantity(int cartItemId)
    {
    var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

    if (cartItem == null)
    {
        return NotFound(new
        {
            message = "Ürün bulunamadı."
        });
    }

    if (cartItem.Quantity > 1)
    {
        cartItem.Quantity--;
    }
    else
    {
        await _cartRepository.RemoveCartItemAsync(cartItem);
    }

    await _cartRepository.SaveChangesAsync();

    return Ok(new
    {
        message = "Sepet güncellendi."
    });
}
}