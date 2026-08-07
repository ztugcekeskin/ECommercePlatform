using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("OrderItems")]
public class OrderItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("orderid")]
    public int OrderId { get; set; }
    [Column("productid")]
    public int ProductId { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("unitprice")]
    public decimal UnitPrice { get; set; }
    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}