namespace MinecraftE_Commerce.Domain.Models
{
    public class Sale
    {
        public int Id { get; set; }

        //Receiver configs
        public string ReceiverId { get; set; } = null!;
        public User Receiver { get; set; } = null!;

        //Buyer configs

        public string BuyerId { get; set; } = null!;
        public User Buyer { get; set; } = null!;

        //Announcement configs
        public int AnnouncementId { get; set; }
        public Announcement Announcement { get; set; } = null!;

        public DateTime SaledOn { get; set; } = DateTime.Now;
        public decimal AnnouncementPrice { get; set; }
    }
}
