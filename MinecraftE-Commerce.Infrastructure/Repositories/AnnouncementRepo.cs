using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infrastructure.Data;

namespace MinecraftE_Commerce.Infrastructure.Repositories
{
    public class AnnouncementRepo : IAnnoucementService
    {
        private readonly AppDbContext _context;

        public AnnouncementRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Announcement> CreateAnnouncements(Announcement annModel)
        {

            await _context.Announcements.AddAsync(annModel);
            await _context.SaveChangesAsync();
            
            return annModel;
        }

        public async Task<Announcement> DeleteAnnouncement(int id)
        {
            var annoucementId = await _context.Announcements.FindAsync(id);

            if (annoucementId == null)
            {
                return null!;
            }

            _context.Announcements.Remove(annoucementId!);
            await _context.SaveChangesAsync();
            return annoucementId;
        }

        public async Task<Announcement> EditAnnouncemenet(Announcement annModel, int id)
        {
            var annoucementId = await _context.Announcements.FindAsync(id);

            if (annoucementId == null) return null!;

            annoucementId.Title = annModel.Title;
            annoucementId.Descripton = annModel.Descripton;
            annoucementId.PriceService = annModel.PriceService;
            annoucementId.ImageAnnouncement = annModel.ImageAnnouncement;

            await _context.SaveChangesAsync();

            return annoucementId;
        }

        public async Task<List<Announcement>> GetAllAnnouncements()
        {
            var returnGet = await _context.Announcements.ToListAsync();
            return returnGet;
        }

        public async Task<Announcement> GetAnnouncementById(int id)
        {
            var search = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);
            if (search == null) return null!;
            return search;
        }
    }
}
