using System.ComponentModel.DataAnnotations;
namespace WebAPI.DTOs
{
    public class UserDto
    {
        [Required]  
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Gender { get; set; }
        [Required]
        public required int Age { get; set; }
        [Required]
        public required string Role { get; set; }
        [Required]  
        public required string Email { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}