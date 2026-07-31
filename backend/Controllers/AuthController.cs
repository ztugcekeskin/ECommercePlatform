using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.DTOs;
using System.Text.RegularExpressions;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers ;
[ApiController]
[Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDto dto)
{
    if (!Regex.IsMatch(dto.Email,
        @"^[a-zA-Z0-9._%+]+@(gmail|hotmail|outlook)\.com$"))
    {
        return BadRequest(new
        {
            message = "Sadece @gmail.com, @hotmail.com veya @outlook.com kullanılabilir."
        });
    }

    var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);

    if (existingUser != null)
    {
        return BadRequest(new
        {
            message = "Bu kullanıcı adı zaten kullanılıyor."
        });
    }

    var user = new User
    {
        FirstName = dto.Name,
        LastName = dto.Surname,
        Age = dto.Age,
        Gender = dto.Gender,
        Email = dto.Email,
        Username = dto.Username,
        Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Role = dto.Role,
        ImageUrl = null
    };

    await _userRepository.AddAsync(user);
    await _userRepository.SaveChangesAsync();

    return Ok(new
    {
        message = "Kayıt başarılı."
    });
}

    [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    var user = await _userRepository.GetByUsernameAsync(request.Username);

    if (user == null)
    {
        return Unauthorized(new
        {
            message = "Kullanıcı adı veya şifre yanlış."
        });
    }

    bool passwordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

    if (!passwordCorrect)
    {
        return Unauthorized(new
        {
            message = "Kullanıcı adı veya şifre yanlış."
        });
    }

    return Ok(new
    {
        message = "Giriş başarılı.",
        user = new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username,
            user.Email,
            user.Role,
            user.Age,
            user.Gender,
            user.ImageUrl
        }
    });
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateProfile(int id, UserDto dto)
{
    var user = await _userRepository.GetByIdAsync(id);

    if (user == null)
    {
        return NotFound(new
        {
            message = "Kullanıcı bulunamadı."
        });
    }

    user.FirstName = dto.FirstName;
    user.LastName = dto.LastName;
    user.Age = dto.Age;
    user.Gender = dto.Gender;
    user.Email = dto.Email;
    user.Username = dto.Username;
    user.Role = dto.Role;

  
    user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

    await _userRepository.Update(user);

    return Ok(new
    {
        message = "Profil başarıyla güncellendi."
    });
}

}