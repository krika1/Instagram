﻿namespace Meta.Instagram.Infrastructure.Entities
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
    }
}
