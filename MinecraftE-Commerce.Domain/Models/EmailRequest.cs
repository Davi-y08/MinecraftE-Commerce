namespace MinecraftE_Commerce.Domain.Models
{
    public class EmailRequest
    {
        public string? ToEmail { get; set; }
        public string Sebject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
