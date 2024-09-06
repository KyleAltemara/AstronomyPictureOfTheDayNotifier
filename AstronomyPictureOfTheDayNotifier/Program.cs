using HtmlAgilityPack;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AstronomyPictureOfTheDayNotifier
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var url = "https://apod.nasa.gov/apod/astropix.html";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var imageName = doc.DocumentNode.SelectSingleNode("/html/body/center[2]/b[1]").InnerText;
            var imageUrl = doc.DocumentNode.SelectSingleNode("//a[contains(@href, 'image')]/img").Attributes["src"].Value;
            var fullImageUrl = $"https://apod.nasa.gov/apod/{imageUrl}";
            var imageFileName = Path.GetFileName(fullImageUrl);

            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(fullImageUrl);
                await File.WriteAllBytesAsync(imageFileName, imageBytes);
            }

            var emailSettings = GetEmailSettings();
            if (emailSettings is null)
            {
                return;
            }

            var smtpClient = new SmtpClient(emailSettings.Host)
            {
                Port = emailSettings.Port,
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
                EnableSsl = true,
            };

            using (MailMessage mail = new())
            {
                mail.From = new MailAddress(emailSettings.Sender);
                mail.To.Add(emailSettings.Recipient);
                mail.Subject = $"{DateTime.Now.ToShortDateString()} Astronomy Picture of the Day: {imageName}";
                var bodyHtml = doc.DocumentNode.InnerHtml.Replace(imageUrl, $"cid:{imageFileName}");
                bodyHtml = UpdateHrefLinks(bodyHtml);

                mail.Body = bodyHtml;
                mail.IsBodyHtml = true;
                mail.Attachments.Add(new Attachment(imageFileName) { ContentId = imageFileName });
                smtpClient.Send(mail);
            }

            File.Delete(imageFileName);
            Console.WriteLine("Sent");
        }

        private static EmailSettings? GetEmailSettings()
        {
            try
            {
                var configPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "appconfig.json");
                var config = JsonSerializer.Deserialize<Dictionary<string, EmailSettings>>(File.ReadAllText(configPath));
                if (config != null && config.TryGetValue("SmtpClient", out var emailSettings))
                {
                    return emailSettings;
                }

                Console.WriteLine("SmtpClient configuration not found or invalid");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading configuration: {ex.Message}");
                return null;
            }
        }

        public static string UpdateHrefLinks(string body)
        {
            // This pattern matches href attributes that do not start with "http"
            string pattern = @"href\s*=\s*['""]((?!http)[^'""]+)['""]";
            /* - href\s*=\s* : Matches the literal string "href" followed by zero or more whitespace characters, an equals sign, and zero or more whitespace characters.
               - ['""] : Matches either a single quote or double quote.
               - ((?!http)[^'""]+) : A capturing group that matches any sequence of characters that does not start with "http". 
                   - (?!http) : A negative lookahead that ensures the sequence does not start with "http".
                   - [^'""]+ : Matches one or more characters that are not single or double quotes.
               - ['""] : Matches either a single quote or double quote to close the href attribute. */

            // This replacement string prepends "https://apod.nasa.gov/apod/" to the matched href value
            string replacement = @"href='https://apod.nasa.gov/apod/$1'";
            /* - href='https://apod.nasa.gov/apod/ : The literal string to prepend to the href value.
               - $1 : Refers to the first capturing group in the pattern, which is the href value that does not start with "http". */
            return Regex.Replace(body, pattern, replacement);
        }
    }
}
