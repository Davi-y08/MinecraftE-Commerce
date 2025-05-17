using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper
{
    public static class DisplayAnnouncement
    {
        public static AnnouncementDisplay MapToDisplay(this Announcement addModel)
        {
            return new AnnouncementDisplay
            {
                Title = addModel.Title,
                Description = addModel.Descripton,
                PriceService = addModel.PriceService,
                Images = addModel.Images?.ToList() ?? new List<ImagesAnnouncement>(),
                CreatedAt = addModel.CreatedAt,
                UserName = addModel.UserName,
                UserPfp = addModel.UserPfp,
                TypeAnnouncement = addModel.TypeOfAnnouncement,
            };
        }
    }
}
