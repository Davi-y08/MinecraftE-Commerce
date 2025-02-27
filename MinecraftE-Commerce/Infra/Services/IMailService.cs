namespace MinecraftE_Commerce.Infra.Services
{
    public interface IMailService
    {
        void SendEmail(string email, string subject, string body, bool ishtml = false);
    }
}
