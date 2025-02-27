using System.Net;
using System.Net.Mail;

namespace MinecraftE_Commerce.Infra.Services
{
    public class MailSender : IMailService
    {
        private string smtpAddress => "smtp.office365.com";
        private int portNumber => 587;
        private string EmailFromAddress => "";
        private string password => "";

        public void SendEmail(string email, string subject, string body, bool ishtml = false)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(EmailFromAddress);
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = ishtml;

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.EnableSsl = true;  
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(EmailFromAddress, password);
                    smtp.Send(mailMessage);
                }
            }
        }
    }
}
