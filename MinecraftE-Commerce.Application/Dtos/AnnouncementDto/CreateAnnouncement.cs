using Microsoft.AspNetCore.Http;

namespace MinecraftE_Commerce.Application.Dtos.AnnouncementDto
{
    public class CreateAnnouncement
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public required IFormFile ImageAnnouncement { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
