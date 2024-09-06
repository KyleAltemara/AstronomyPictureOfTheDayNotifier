namespace AstronomyPictureOfTheDayNotifier
{
    internal class Program
    {
        static async Task Main()
        {
            var (imageName, imageFileName, bodyHtml) = await AstronomyPictureService.FetchAstronomyPictureAsync();

            var emailSettings = ConfigurationService.GetEmailSettings();
            if (emailSettings is null)
            {
                return;
            }

            var emailService = new EmailService(emailSettings);
            emailService.SendEmail($"{DateTime.Now.ToShortDateString()} Astronomy Picture of the Day: {imageName}", bodyHtml, imageFileName);

            File.Delete(imageFileName);
            Console.WriteLine("Sent");
        }
    }
}
