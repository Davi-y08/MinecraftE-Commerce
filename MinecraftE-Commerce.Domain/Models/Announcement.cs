using System.Text.Json.Serialization;
using MinecraftE_Commerce.Domain.Enums.AnnouncementsEnums;

namespace MinecraftE_Commerce.Domain.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Descripton { get; set; } = string.Empty;
        public string ImageAnnouncement { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal PriceService { get; set; }
        public int Sales { get; set; } = 0;
        [JsonIgnore]
        public User? UserInfo { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserPfp { get; set; } = string.Empty;
        public int Clicks { get; set; } = 0;
        public TypeOfAnnouncementEnum TypeOfAnnouncement { get; set; } 
    }
}
