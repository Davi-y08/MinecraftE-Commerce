namespace MinecraftE_Commerce.Domain.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public Sale Sale { get; set; }
        public int SaleId { get; set; }
        public User Receiver { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public User Buyer { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public List<Message> MyProperty { get; set; }
    }
}
