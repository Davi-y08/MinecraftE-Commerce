namespace MinecraftE_Commerce.Domain.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public int ReceiverId { get; set; }
        public int BuyerId { get; set; }
        public User User { get; set; } = null!;
        public Announcement Announcement { get; set; } = null!;
        public DateTime SaledOn { get; set; } = DateTime.Now;
        public decimal AnnouncementPrice { get; set; }
    }
}
