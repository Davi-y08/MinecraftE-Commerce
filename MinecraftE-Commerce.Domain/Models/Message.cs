namespace MinecraftE_Commerce.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageString { get; set; } = string.Empty;
        public DateTime Send_at { get; set; } = DateTime.Now;
        public User User { get; set; }
        public string UserId { get; set; }
        public Chat Chat { get; set; }
        public int ChatId { get; set; }
    }
}
