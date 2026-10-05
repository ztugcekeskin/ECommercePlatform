using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("Payments")]
public class Payment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("orderid")]
    public int OrderId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("paymentmethod")]
    public string PaymentMethod { get; set; } = "Card";

    [Column("status")]
    public string Status { get; set; } = "Pending";

    [Column("transactionid")]
    public string? TransactionId { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
}