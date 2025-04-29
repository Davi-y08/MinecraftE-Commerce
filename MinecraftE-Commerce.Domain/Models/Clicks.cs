namespace MinecraftE_Commerce.Domain.Models
{
    public class Clicks
    {
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Announcement? Announcement { get; set; }
    }
}
