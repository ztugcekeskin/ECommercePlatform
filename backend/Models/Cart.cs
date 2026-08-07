using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("Cart")]
public class Cart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("customerid")]
    public int CustomerId { get; set; }
    public List<CartItem> CartItems { get; set; } = new();
}