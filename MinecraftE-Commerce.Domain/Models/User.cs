using Microsoft.AspNetCore.Identity;

namespace MinecraftE_Commerce.Domain.Models
{
    public class User : IdentityUser
    {
        public List<Announcement>? Announcements { get; set; } = new List<Announcement>();
        public string Pfp { get; set; } = string.Empty;
    }
}
