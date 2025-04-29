using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Clicks> Clickss { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Announcement>()
                .HasOne(p => p.UserInfo)
                .WithMany(p => p!.Announcements)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sale>()
            .HasOne(p => p.BuyerInfo)
            .WithMany(p => p!.Sales)
            .HasForeignKey(s => s.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sale>()
                .HasOne(p => p.AnnouncementInfo)
                .WithMany()
                .HasForeignKey(s => s.AnnouncementId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
    