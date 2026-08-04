using Microsoft.AspNetCore.Http;

namespace WebAPI.DTOs
{
    public class UploadProfilePhotoDto
    {
        public IFormFile Image { get; set; } = null!;
    }
}