using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("Products")]
public class Product
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("SellerId")]
    public int SellerId { get; set; }

    [Column("Name")]
    public string Name { get; set; } = "";

    [Column("Description")]
    public string Description { get; set; } = "";

    [Column("Price")]
    public decimal Price { get; set; }

    [Column("Stock")]
    public int Stock { get; set; }

    [Column("ImageUrl")]
    public string? ImageUrl { get; set; }
}