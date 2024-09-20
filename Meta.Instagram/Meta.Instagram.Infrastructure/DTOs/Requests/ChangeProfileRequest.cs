using Microsoft.AspNetCore.Http;

namespace Meta.Instagram.Infrastructure.DTOs.Requests
{
    public class ChangeProfileRequest
    {
        public IFormFile? Picture { get; set; }
        public string? Description { get; set; }
        public bool? IsPublic { get; set; }
    }
}
