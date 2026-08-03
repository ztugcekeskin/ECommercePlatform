namespace WebAPI.DTOs;

public class AddToCartDto
{
    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}