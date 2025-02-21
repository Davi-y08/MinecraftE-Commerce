using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper
{
    public static class DisplayAnnouncement
    {
        public static AnnouncementDisplay MapToDisplay(Announcement addModel)
        {
            return new AnnouncementDisplay
            {
                Title = addModel.Title,
                Description = addModel.Descripton,
                PriceService = addModel.PriceService,
                ImageAnnouncement = addModel.ImageAnnouncement,
                CreatedAt = addModel.CreatedAt,
            };
        }
    }
}
