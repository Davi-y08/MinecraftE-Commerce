﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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

        public async Task<int> ClicksInMounth(string idUser)
        {
            var trintaDiasAtras = DateTime.UtcNow.AddDays(-30);

            var totalClicks = await _context.Clickss.Where(c => c.Announcement!.UserId 
            == idUser 
            && 
            c.CreatedAt 
            >= trintaDiasAtras)
                .CountAsync();

            return totalClicks;
        }

        public async Task<Announcement> CreateAnnouncements(Announcement annModel)
        {
            if (annModel.Images != null && annModel.Images.Any())
            {
                foreach (var image in annModel.Images)
                {
                    image.Announcement = annModel;
                }
            }

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
            annoucementId.Images = annModel.Images;

            await _context.SaveChangesAsync();

            return annoucementId;
        }

        public async Task<List<Announcement>> GetAllAnnouncements()
        {
            var returnGet = await _context.Announcements.Include(a => a.Images).ToListAsync();

            return returnGet;
        }

        public async Task<Announcement> GetAnnouncementById(int id)
        {
            var search = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);
            if (search == null) return null!;
            return search;
        }

        public async Task<List<Announcement>> MyAnnouncement(string idUser)
        {
            List<Announcement> announcement = await _context.Announcements.Where(c => c.UserId == idUser).ToListAsync();

            if (announcement == null) return null!;

            return announcement;
        }

        public async Task<Announcement> ReadAndAddValueForSales(int id)
        {
            var announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);

            if (announcement == null)
                return null!;

            announcement.Sales += 1;

            await _context.SaveChangesAsync();

            return announcement;
        }
    }
}
