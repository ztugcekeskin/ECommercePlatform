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
    var errors = new List<string>();

    if (string.IsNullOrWhiteSpace(dto.Username))
        errors.Add("Kullanıcı adı boş bırakılamaz.");

    if (dto.Username.Length < 2 || dto.Username.Length > 30)
        errors.Add("Kullanıcı adı 2-30 karakter arasında olmalıdır.");

if (!Regex.IsMatch(dto.Username, @"^[a-zA-Z0-9.!()_]+$"))
    errors.Add("Kullanıcı adı sadece harf, rakam ve şu özel karakterleri içerebilir: . ! ( ) _");

    if (!Regex.IsMatch(dto.Email,
        @"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook)\.com$"))
    {
        errors.Add("Sadece @gmail.com, @hotmail.com veya @outlook.com kullanılabilir.");
    }

    if (string.IsNullOrWhiteSpace(dto.Password))
        errors.Add("Şifre boş bırakılamaz.");
    else
    {
        if (dto.Password.Length < 8)
            errors.Add("Şifre en az 8 karakter olmalıdır.");

        if (!Regex.IsMatch(dto.Password, @"[A-Z]"))
            errors.Add("Şifre en az 1 büyük harf içermelidir.");

        if (!Regex.IsMatch(dto.Password, @"[a-z]"))
            errors.Add("Şifre en az 1 küçük harf içermelidir.");

        if (!Regex.IsMatch(dto.Password, @"[0-9]"))
            errors.Add("Şifre en az 1 rakam içermelidir.");

        if (!Regex.IsMatch(dto.Password, @"[\W_]"))
            errors.Add("Şifre en az 1 özel karakter içermelidir.");
    }

    if (errors.Any())
    {
        return BadRequest(new
        {
            message = "Lütfen hataları düzeltin.",
            errors
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
}