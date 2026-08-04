using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebAPI.DTOs;
using WebAPI.Repositories.Interfaces;
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

    return Ok(new
    {
        message = "Fotoğraf başarıyla yüklendi.",
        fileName = fileName
    });
}

}