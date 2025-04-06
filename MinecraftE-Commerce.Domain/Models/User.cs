using Microsoft.AspNetCore.Identity;

namespace MinecraftE_Commerce.Domain.Models
{
    public class User : IdentityUser
    {
        public List<Announcement>? Announcements { get; set; } 
        public List<Sale>? Sales { get; set; }
        public List<Sale>? Compras { get; set; }
        public string Pfp { get; set; } = string.Empty;
    }
}
