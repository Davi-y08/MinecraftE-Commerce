using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper
{
    public static class EditMapper
    {
        public static Announcement MapToEditAnnouncement(this EditAnnouncement editAdd)
        {
            return new Announcement
            {
                Title = editAdd.Title,
                Descripton = editAdd.Description,
                PriceService = editAdd.PriceService,
                ImageAnnouncement = editAdd.SameImage 
            };
        }
    }
}
