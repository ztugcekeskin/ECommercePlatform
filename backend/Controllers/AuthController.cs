using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.DTOs;
using WebAPI.Services;
using System.Text.RegularExpressions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        var result = _userService.Register(dto);

        if (!Regex.IsMatch(dto.Email,
        @"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook)\.com$"))
        {
            return BadRequest(new
        {
            message = "Sadece @gmail.com, @hotmail.com veya @outlook.com kullanılabilir."
        });
        }
        if (!result)
        {
            return BadRequest(new
            {
                message = "Bu kullanıcı adı zaten kullanılıyor."
            });
        }

        return Ok(new
        {
            message = "Kayıt başarılı."
        });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userService.Login(request);

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Kullanıcı adı veya şifre yanlış."
            });
        }

        return Ok(new
        {
            message = "Giriş başarılı.",
            user
        });
    }
}