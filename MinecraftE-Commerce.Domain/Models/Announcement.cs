namespace MinecraftE_Commerce.Domain.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Descripton { get; set; } = string.Empty;
        public string ImageAnnouncement { get; set; } = string.Empty;
        public User? User { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;    
    }
}
