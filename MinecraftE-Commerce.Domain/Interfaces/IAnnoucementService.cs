using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Interfaces
{
    public interface IAnnoucementService
    {
        Task<List<Announcement>> GetAllAnnouncements();
        Task<Announcement> GetAnnouncementById(int id);
        Task<Announcement> CreateAnnouncements(Announcement annModel);
        Task<Announcement> EditAnnouncemenet(Announcement annModel, int id);
        Task<Announcement> DeleteAnnouncement(int id);
        Task<Announcement> ReadAndAddValueForSales(int id);
    }
}
