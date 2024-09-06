using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace AstronomyPictureOfTheDayNotifier
{
    public class AstronomyPictureService
    {
        public static async Task<(string ImageName, string ImageFileName, string BodyHtml)> FetchAstronomyPictureAsync()
        {
            var url = "https://apod.nasa.gov/apod/astropix.html";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var imageName = doc.DocumentNode.SelectSingleNode("/html/body/center[2]/b[1]").InnerText;
            var imageUrl = doc.DocumentNode.SelectSingleNode("//a[contains(@href, 'image')]/img").Attributes["src"].Value;
            var fullImageUrl = $"https://apod.nasa.gov/apod/{imageUrl}";
            var imageFileName = Path.GetFileName(fullImageUrl);
            var bodyHtml = doc.DocumentNode.InnerHtml.Replace(imageUrl, $"cid:{imageFileName}");
            bodyHtml = UpdateHrefLinks(bodyHtml);

            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(fullImageUrl);
                await File.WriteAllBytesAsync(imageFileName, imageBytes);
            }

            return (imageName, imageFileName, bodyHtml);
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
