using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    [HttpPost("checkout")]
public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
{
    var cart = await _orderRepository.GetCartByCustomerIdAsync(dto.CustomerId);

    if (cart == null || !cart.CartItems.Any())
    {
        return BadRequest(new
        {
            message = "Sepet boş."
        });
    }
    var order = new Order
    {
    CustomerId = dto.CustomerId,
    OrderDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),    Status = "Hazırlanıyor",
    TotalPrice = cart.CartItems.Sum(ci =>
        ci.Product.Price * ci.Quantity)
    };
    await _orderRepository.AddOrderAsync(order);

    await _orderRepository.SaveChangesAsync();
    
    foreach (var item in cart.CartItems)
{
    var orderItem = new OrderItem
{
    OrderId = order.Id,
    ProductId = item.ProductId,
    Quantity = item.Quantity,
    UnitPrice = item.Product.Price
};
    await _orderRepository.AddOrderItemAsync(orderItem);    
    
    item.Product.Stock -= item.Quantity;

    await _orderRepository.UpdateProductAsync(item.Product);
}
    await _orderRepository.SaveChangesAsync();

    await _orderRepository.ClearCartAsync(cart);

    await _orderRepository.SaveChangesAsync();

    return Ok(new
{
    message = "Sipariş başarıyla oluşturuldu.",
    orderId = order.Id
});
}

    [HttpGet("{customerId}")]
public async Task<IActionResult> GetOrders(int customerId)
{
    var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);

    var result = orders.Select(order => new
    {
        id = order.Id,
        orderDate = order.OrderDate,
        status = order.Status,
        totalPrice = order.TotalPrice,

        orderItems = order.OrderItems.Select(item => new
        {
            id = item.Id,
            quantity = item.Quantity,
            unitPrice = item.UnitPrice,

            product = new
            {
                id = item.Product.Id,
                name = item.Product.Name,
                description = item.Product.Description,
                imageUrl = item.Product.ImageUrl
            }
        })
    });

    return Ok(result);
}
}