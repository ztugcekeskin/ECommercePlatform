using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebAPI.DTOs;
using WebAPI.Repositories.Interfaces;
using System.Text.RegularExpressions;
using BCrypt.Net;
using System.IO;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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

    var errors = new List<string>();

    if (string.IsNullOrWhiteSpace(dto.Username))
    errors.Add("Kullanıcı adı boş bırakılamaz.");

    if (dto.Username.Length < 2 || dto.Username.Length > 30)
    errors.Add("Kullanıcı adı 2-30 karakter arasında olmalıdır.");

    if (!Regex.IsMatch(dto.Username, @"^[a-zA-Z0-9.!()_]+$"))
    errors.Add("Kullanıcı adı sadece harf, rakam ve şu özel karakterleri içerebilir: . ! ( ) _");

    if (!Regex.IsMatch(dto.Email, @"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook)\.com$"))
    errors.Add("Sadece @gmail.com, @hotmail.com veya @outlook.com kullanılabilir.");

    if (string.IsNullOrWhiteSpace(dto.Password))
    {
    errors.Add("Şifre boş bırakılamaz.");
    }
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

    if (!Regex.IsMatch(dto.Password, @"[.!()_]"))
        errors.Add("Şifre en az 1 özel karakter (. ! ( ) _) içermelidir.");

    if (!Regex.IsMatch(dto.Password, @"^[a-zA-Z0-9.!()_]+$"))
        errors.Add("Şifre sadece harf, rakam ve şu özel karakterleri içerebilir: . ! ( ) _");
    }

    if (errors.Any())
    {
    return BadRequest(new
        {
        message = "Lütfen aşağıdaki hataları düzeltin.",
        errors
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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
    var user = await _userRepository.GetByIdAsync(id);

    if (user == null)
    {
        return NotFound(new
        {
            message = "Kullanıcı bulunamadı."
        });
    }

    return Ok(new
    {
        user.Id,
        user.FirstName,
        user.LastName,
        user.Username,
        user.Email,
        user.Age,
        user.Gender,
        user.Role,
        user.ImageUrl
    });
    }

    [HttpPost("{id}/upload-photo")]
    public async Task<IActionResult> UploadPhoto(
    int id,
    [FromForm] UploadProfilePhotoDto dto)
    {
    var user = await _userRepository.GetByIdAsync(id);

    if (user == null)
    {
        return NotFound(new
        {
            message = "Kullanıcı bulunamadı."
        });
    }

    if (dto.Image == null || dto.Image.Length == 0)
    {
        return BadRequest(new
        {
            message = "Lütfen bir fotoğraf seçin."
        });
    }

    var allowedExtensions = new[] { ".jpg", ".jpeg" };
    var extension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();

    if (!allowedExtensions.Contains(extension))
    {
    return BadRequest(new
    {
        message = "Sadece JPG ve JPEG formatındaki fotoğraflar kabul edilir."
    });
    }

    var uploadsFolder = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Uploads",
        "Profiles");

    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);

    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await dto.Image.CopyToAsync(stream);
    }

    user.ImageUrl = "/uploads/Profiles/" + fileName;
    
    await _userRepository.Update(user);

  return Ok(new
    {
    message = "Fotoğraf başarıyla yüklendi.",
    imageUrl = user.ImageUrl
    });
    }

    [HttpDelete("{id}/photo")]
public async Task<IActionResult> DeletePhoto(int id)
{
    var user = await _userRepository.GetByIdAsync(id);

    if (user == null)
    {
        return NotFound(new
        {
            message = "Kullanıcı bulunamadı."
        });
    }

    user.ImageUrl = null;

    await _userRepository.Update(user);

    return Ok(new
    {
        message = "Profil fotoğrafı silindi."
    });
}


}