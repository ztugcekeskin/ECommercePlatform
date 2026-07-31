using System.ComponentModel.DataAnnotations;
namespace WebAPI.DTOs;

public class RegisterDto
{
    [Required]
     [StringLength(30, MinimumLength = 2)]
         public string Name { get; set; } = "";
    [Required]
     [StringLength(30, MinimumLength = 2)]
       public string Surname { get; set; } = "";
   
    [Required]
    [Range(18, 100)]
    public int Age { get; set; }

    [Required]
    public string Gender { get; set; } = "";
   
    [Required]
    public string Role { get; set; } = "";
   
    [Required]
    [EmailAddress]
     [RegularExpression(
        @"^[A-Za-z0-9]+@(gmail\.com|hotmail\.com|outlook\.com)$",
        ErrorMessage = "Sadece Gmail, Hotmail veya Outlook adresleri kullanılabilir.")]
    public string Email { get; set; } = "";
    [Required]
     [StringLength(20, MinimumLength = 4)]
    public string Username { get; set; } = "";
    [Required]
      [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
        ErrorMessage = "Şifre en az 8 karakter, bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.")]
    public string Password { get; set; } = "";
}