namespace MinecraftE_Commerce.Domain.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public Sale Sale { get; set; }
        public int SaleId { get; set; }
        public User UserReceiver { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public User UserSender { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public List<Message> MyProperty { get; set; }
    }
}
