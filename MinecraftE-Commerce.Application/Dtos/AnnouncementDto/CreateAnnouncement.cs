using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MinecraftE_Commerce.Application.Dtos.AnnouncementDto
{
    public class CreateAnnouncement
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image Announcement is required")]
        public required IFormFile ImageAnnouncement { get; set; }
        public decimal PriceService { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
