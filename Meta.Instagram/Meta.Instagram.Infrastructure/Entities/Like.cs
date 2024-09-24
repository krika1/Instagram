namespace Meta.Instagram.Infrastructure.Entities
{
    public class Like
    {
        public string? LikeId { get; set; }
        public string? ProfileId { get; set; }
        public string? PictureId { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual Picture? Picture { get; set; }
    }
}
