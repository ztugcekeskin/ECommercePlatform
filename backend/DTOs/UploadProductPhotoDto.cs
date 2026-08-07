using Microsoft.AspNetCore.Http;

namespace WebAPI.DTOs;

public class UploadProductPhotoDto
{
 public IFormFile? Image { get; set; }
}
