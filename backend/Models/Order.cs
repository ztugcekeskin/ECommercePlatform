using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("Orders")]
public class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("customerid")]
    public int CustomerId { get; set; }
    [Column("orderdate")]
    public DateTime OrderDate { get; set; }
    [Column("status")]
    public string Status { get; set; } = "Hazırlanıyor";
    [Column("totalprice")]
    public decimal TotalPrice { get; set; }
    [ForeignKey(nameof(CustomerId))]
    public User Customer { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = new();
}