namespace MinecraftE_Commerce.Domain.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public string BuyerId { get; set; } = string.Empty;
        public User? BuyerInfo { get; set; }
        public int AnnouncementId { get; set; }
        public Announcement AnnouncementInfo { get; set; } = null!;
        public DateTime SaledOn { get; set; } = DateTime.Now;
        public decimal AnnouncementPrice { get; set; }
    }
}