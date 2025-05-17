using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MinecraftE_Commerce.Domain.Enums.AnnouncementsEnums;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Dtos.AnnouncementDto
{
    public class CreateAnnouncement
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal PriceService { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "The type of announcement is obrigatory")]
        public TypeOfAnnouncementEnum TypeAnnouncement { get; set; }

        [Required]
        public List<IFormFile> ImagesAnnouncements { get; set; }
    }
}
