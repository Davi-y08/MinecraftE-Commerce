using System.Text.Json.Serialization;
using MinecraftE_Commerce.Domain.Enums.AnnouncementsEnums;

namespace MinecraftE_Commerce.Domain.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Descripton { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal PriceService { get; set; }
        public int Sales { get; set; } = 0;
        [JsonIgnore]
        public User? UserInfo { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserPfp { get; set; } = string.Empty;
        public ICollection<Clicks> Clickss { get; set; }
        public TypeOfAnnouncementEnum TypeOfAnnouncement { get; set; }
        public ICollection<ImagesAnnouncement> Images { get; set; }
    }

    public class ImagesAnnouncement
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public Announcement Announcement { get; set; }
        public int AnnouncementId { get; set; }

    }
}
