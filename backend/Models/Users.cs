using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("users")]
public class User
{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Email { get; set; } = "";

    public string Password { get; set; } = "";
    public string Username { get; set; } = "";
    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string Role { get; set; } = "Customer";

    public string? ImageUrl { get; set; }
    public List<Order> Orders { get; set; } = new();
}