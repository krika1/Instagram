using Meta.Instagram.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Meta.Instagram.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateAccountModelBuilder(modelBuilder);
            CreateProfileModelBuilder(modelBuilder);
            CreateProfileFollowModelBuilder(modelBuilder);
            CreatePictureModelBuilder(modelBuilder);
            CreateLikeModelBuilder(modelBuilder);
        }

        private void CreateLikeModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
            .HasKey(l => l.LikeId);                

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Profile)
                .WithMany() 
                .HasForeignKey(l => l.ProfileId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Picture)
                .WithMany(p => p.Likes) 
                .HasForeignKey(l => l.PictureId);
        }

        private void CreatePictureModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasKey(a => a.PictureId);

                entity.Property(a => a.PictureId)
                      .IsRequired();

                entity.Property(a => a.PicturePath)
                    .IsRequired();

                entity.Property(a => a.Descripton)
                      .IsRequired();

                entity.Property(a => a.UploadAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(p => p.Profile)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(p => p.ProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void CreateProfileFollowModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follow>()
                .HasKey(pf => new { pf.FollowerId, pf.FollowingId });

            modelBuilder.Entity<Follow>()
                .HasOne(pf => pf.Follower)
                .WithMany(p => p.Following)
                .HasForeignKey(pf => pf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(pf => pf.Following)
                .WithMany(p => p.Followers)
                .HasForeignKey(pf => pf.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void CreateProfileModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(a => a.ProfileId);

                entity.Property(a => a.ProfileId)
                      .IsRequired();

                entity.Property(a => a.AccountId)
                    .IsRequired();

                entity.Property(a => a.Username)
                      .IsRequired();

                entity.Property(a => a.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(a => a.UpdatedAt)
                      .IsRequired(false);

                entity.HasOne(p => p.Account)
                     .WithOne(a => a.Profile)
                     .HasForeignKey<Profile>(p => p.AccountId)
                     .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void CreateAccountModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.AccountId);

                entity.Property(a => a.AccountId)
                      .IsRequired();

                entity.Property(a => a.ExternalId)
                      .IsRequired();

                entity.Property(a => a.FirstName)
                      .IsRequired();

                entity.Property(a => a.LastName)
                      .IsRequired();

                entity.Property(a => a.Username)
                      .IsRequired();

                entity.Property(a => a.Email)
                      .IsRequired();

                entity.Property(a => a.Phone)
                      .IsRequired();

                entity.Property(a => a.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(a => a.UpdatedAt)
                      .IsRequired(false);
            });
        }
    }
}
