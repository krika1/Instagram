using Microsoft.AspNetCore.Http;

namespace Meta.Instagram.Infrastructure.DTOs.Requests
{
    public class UploadPictureRequest
    {
        public IFormFile? Picture { get; set; }
        public string? Description { get; set; }
    }
}
