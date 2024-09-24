namespace Meta.Instagram.Infrastructure.Entities
{
    public class Follow
    {
        public string FollowerId { get; set; } = null!;
        public Profile Follower { get; set; } = null!;

        public string FollowingId { get; set; } = null!;
        public Profile Following { get; set; } = null!;
    }
}
