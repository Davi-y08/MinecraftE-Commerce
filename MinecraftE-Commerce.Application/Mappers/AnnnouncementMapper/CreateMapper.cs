﻿using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper
{
    public static class CreateMapper
    {
        public static Announcement MapToCreateAnnouncement(this CreateAnnouncement createAdd)
        {

            return new Announcement
            {
                Title = createAdd.Title,
                Descripton = createAdd.Description,
                CreatedAt = createAdd.CreatedAt,
                Images = new List<ImagesAnnouncement>(),
                PriceService = createAdd.PriceService,
                TypeOfAnnouncement = createAdd.TypeAnnouncement,
            };
        }
    }
}
