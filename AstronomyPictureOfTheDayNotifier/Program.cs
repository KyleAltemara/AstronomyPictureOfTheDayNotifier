using HtmlAgilityPack;

namespace AstronomyPictureOfTheDayNotifier
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var url = "https://apod.nasa.gov/apod/astropix.html";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var imageName = doc.DocumentNode.SelectSingleNode("/html/body/center[2]/b[1]").InnerText;
            Console.WriteLine(imageName);
        }
    }
}
