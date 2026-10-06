using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;

    public PaymentController(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        ICartRepository cartRepository)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
    }

    [HttpPost("create")]
public async Task<IActionResult> CreatePayment([FromBody] PaymentDto dto)
{
    var cart = await _cartRepository.GetCartWithProductsAsync(dto.CustomerId);

    if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
    {
        return BadRequest(new
        {
            message = "Sepet boş."
        });
    }

    var totalAmount = cart.CartItems.Sum(item =>
        item.Product.Price * item.Quantity);

    // Sipariş oluştur
    var order = new Order
    {
        CustomerId = dto.CustomerId,
        OrderDate = DateTime.UtcNow,
        Status = "Ödeme Bekleniyor",
        TotalPrice = totalAmount
    };

    await _orderRepository.AddOrderAsync(order);
    await _orderRepository.SaveChangesAsync();

    // Ödeme kaydı oluştur
    var payment = new Payment
    {
        OrderId = order.Id,
        Amount = totalAmount,
        PaymentMethod = "Card",
        Status = "Pending",
        CreatedAt = DateTime.UtcNow
    };

    await _paymentRepository.AddAsync(payment);

    return Ok(new PaymentResponseDto
    {
        OrderId = order.Id,
        Amount = totalAmount,
        Status = payment.Status,
        TransactionId = payment.TransactionId,
        Message = "Ödeme işlemi başlatıldı."
    });
}

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrderId(int orderId)
    {
        var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

        if (payment == null)
        {
            return NotFound(new
            {
                message = "Bu siparişe ait ödeme bulunamadı."
            });
        }

        return Ok(payment);
    }
}