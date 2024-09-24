namespace Meta.Instagram.Infrastructure.DTOs.Contracts
{
    public class PictureContract
    {
        public string? PictureId { get; set; }
        public string? PicturePath { get; set; }
        public string? Descripton { get; set; }
        public DateTime UploadAt { get; set; }

        public int Likes { get; set; }
    }
}
