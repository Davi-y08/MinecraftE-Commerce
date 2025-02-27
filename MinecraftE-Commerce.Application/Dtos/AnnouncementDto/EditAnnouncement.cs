using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MinecraftE_Commerce.Application.Dtos.AnnouncementDto
{
    public class EditAnnouncement
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal PriceService { get; set; }
        public IFormFile? ImageAnnouncement { get; set; } = null;
        public string SameImage { get; set; } = string.Empty;
    }
}
