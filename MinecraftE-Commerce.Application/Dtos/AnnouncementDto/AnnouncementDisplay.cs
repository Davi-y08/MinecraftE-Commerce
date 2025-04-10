using MinecraftE_Commerce.Domain.Enums.AnnouncementsEnums;

namespace MinecraftE_Commerce.Application.Dtos.AnnouncementDto
{
    public  class AnnouncementDisplay
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PriceService { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageAnnouncement { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserPfp { get; set; } = string.Empty;
        public TypeOfAnnouncementEnum TypeAnnouncement { get; set; }
    }
}
