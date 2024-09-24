namespace Meta.Instagram.Infrastructure.DTOs.Contracts
{
    public class ProfileContract
    {
        public string? ProfileId { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }
        public string? PicturePath { get; set; }
        public bool IsPublic { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }

        public IEnumerable<PictureContract>? Pictures { get; set; }
    }
}
