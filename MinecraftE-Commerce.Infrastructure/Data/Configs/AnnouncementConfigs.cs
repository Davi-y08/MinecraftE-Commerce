using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Infrastructure.Data.Configs
{
    public class AnnouncementConfigs
    {
        public static void SetupAnnouncements(ModelBuilder builder)
        {
            builder.Entity<Announcement>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Title).IsRequired();
                entity.Property(x => x.Descripton).IsRequired();
                entity.Property(x => x.ImageAnnouncement).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });
        }
    }
}
