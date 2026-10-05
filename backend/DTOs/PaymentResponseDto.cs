namespace WebAPI.DTOs;

public class PaymentResponseDto
{
    public int OrderId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? TransactionId { get; set; }

    public string Message { get; set; } = string.Empty;
}