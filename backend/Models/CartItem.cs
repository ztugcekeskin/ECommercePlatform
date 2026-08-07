using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("CartItems")]
public class CartItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("cartid")]
    public int CartId { get; set; }
    [Column("productid")]
    public int ProductId { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;
}