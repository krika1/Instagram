namespace Meta.Instagram.Infrastructure.Entities
{
    public class Profile
    {
        public string? ProfileId { get; set; }
        public string? Username { get; set; }
        public string? AccountId { get; set; }
        public Account? Account { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? PicturePath { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<Follow>? Followers { get; set; }
        public ICollection<Follow>? Following { get; set; }
    }
}
