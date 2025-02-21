using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Services
{
    public class AnnouncementService
    {
        private readonly IAnnoucementService _annService;

        public AnnouncementService(IAnnoucementService annService)
        {
            _annService = annService;
        } 
        
        public async Task<List<Announcement>> GetAllAnnouncements()
        {
            return await _annService.GetAllAnnouncements();
        }

        public async Task<Announcement> GetAnnouncementById(int id)
        {
            return await GetAnnouncementById(id);
        }

        public async Task<Announcement> CreateAnnouncements(Announcement announcement)
        {
            var newAnn = new Announcement();
            newAnn.Id = announcement.Id;
            newAnn.UserId = announcement.UserId;
            newAnn.UserName = announcement.UserName;
            newAnn.ImageAnnouncement = announcement.ImageAnnouncement;
            newAnn.Title = announcement.Title;
            newAnn.Descripton = announcement.Descripton;

            return await _annService.CreateAnnouncements(newAnn);
        }

        public async Task<Announcement> EditAnnouncemenet(Announcement announcement, int id)
        {
            var newAnn = new Announcement();
            newAnn.Id = announcement.Id;
            newAnn.UserId = announcement.UserId;
            newAnn.UserName = announcement.UserName;
            newAnn.ImageAnnouncement = announcement.ImageAnnouncement;
            newAnn.Title = announcement.Title;
            newAnn.Descripton = announcement.Descripton;

            return await _annService.EditAnnouncemenet(newAnn, id);
        }

        public async Task<Announcement> DeleteAnnouncement(int id)
        {
            return await _annService.DeleteAnnouncement(id);    
        }
    }
}
