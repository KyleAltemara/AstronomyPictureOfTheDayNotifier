using System.Net;
using System.Net.Mail;

namespace AstronomyPictureOfTheDayNotifier
{
    public class EmailService(EmailSettings emailSettings)
    {
        private readonly EmailSettings _emailSettings = emailSettings;

        public void SendEmail(string subject, string bodyHtml, string imageFileName)
        {
            var smtpClient = new SmtpClient(_emailSettings.Host)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true,
            };

            using MailMessage mail = new();
            mail.From = new MailAddress(_emailSettings.Sender);
            mail.To.Add(_emailSettings.Recipient);
            mail.Subject = subject;
            mail.Body = bodyHtml;
            mail.IsBodyHtml = true;
            mail.Attachments.Add(new Attachment(imageFileName) { ContentId = imageFileName });
            smtpClient.Send(mail);
        }
    }
}
