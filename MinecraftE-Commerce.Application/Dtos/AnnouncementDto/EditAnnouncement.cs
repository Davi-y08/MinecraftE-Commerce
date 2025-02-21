namespace MinecraftE_Commerce.Application.Dtos.AnnouncementDto
{
    public class EditAnnouncement
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
