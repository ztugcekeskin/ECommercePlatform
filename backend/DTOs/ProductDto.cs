using System.ComponentModel.DataAnnotations;
namespace WebAPI.DTOs;

public class ProductDto
{
    public int SellerId { get; set; }
    [Required]

    public string Name { get; set; } = "";
    [Required]

    public string Description { get; set; } = "";
    [Required]

    public decimal Price { get; set; }
    [Required]

    public int Stock { get; set; }
    [Required]
    public string? ImageUrl { get; set; }
}