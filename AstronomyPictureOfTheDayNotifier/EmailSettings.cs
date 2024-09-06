namespace AstronomyPictureOfTheDayNotifier
{
    public class EmailSettings
    {
        public required string Host { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Sender { get; set; }
        public required string Recipient { get; set; }
        public required int Port { get; set; }
    }
}
