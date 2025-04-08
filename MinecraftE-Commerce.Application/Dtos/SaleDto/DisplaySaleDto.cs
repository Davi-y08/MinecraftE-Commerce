namespace MinecraftE_Commerce.Application.Dtos.SaleDto
{
    public class DisplaySaleDto
    {
        public int idSale { get; set; }
        public int announcementId { get; set; }
        public string buyerId { get; set; } = string.Empty;
        public string receiverId { get; set; } = string.Empty;
        public DateTime saledOn { get; set; }
        public decimal valueAnnouncement { get; set; }
    }
}
