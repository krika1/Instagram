namespace Meta.Instagram.Infrastructure.Entities
{
    public class Picture
    {
        public string? PictureId { get; set; }
        public string? PicturePath { get; set; }
        public string? Descripton { get; set; }
        public DateTime UploadAt { get; set; }

        public string ProfileId { get; set; } = null!;
        public Profile Profile { get; set; } = null!;

        public ICollection<Like> Likes { get; set; } = [];
    }
}
