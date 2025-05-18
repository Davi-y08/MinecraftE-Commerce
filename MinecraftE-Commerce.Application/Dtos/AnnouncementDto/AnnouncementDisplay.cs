using MinecraftE_Commerce.Application.Dtos.ImageDto;
using MinecraftE_Commerce.Domain.Enums.AnnouncementsEnums;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Dtos.AnnouncementDto
{
    public  class AnnouncementDisplay
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PriceService { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ImageDtoDisplay> Images { get; set; } = new();
        public string UserName { get; set; } = string.Empty;
        public string UserPfp { get; set; } = string.Empty;
        public TypeOfAnnouncementEnum TypeAnnouncement { get; set; }
    }
}
