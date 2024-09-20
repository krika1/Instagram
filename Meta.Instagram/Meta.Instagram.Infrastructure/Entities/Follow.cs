namespace Meta.Instagram.Infrastructure.Entities
{
    public class Follow
    {
        public string? FollowerId { get; set; }
        public Profile? Follower { get; set; }

        public string? FollowingId { get; set; }
        public Profile? Following { get; set; }
    }
}
