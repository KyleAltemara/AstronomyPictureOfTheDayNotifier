# AstronomyPictureOfTheDayNotifier

This is a console application that fetches the Astronomy Picture of the Day from NASA's APOD website and sends it via email. The application uses HTML parsing to extract the image and its details, and SMTP to send the email.

Inspiration of the project: <https://www.thecsharpacademy.com/project/19/sports-results>

## Features

- Fetches the Astronomy Picture of the Day from NASA's APOD website.
- Parses the HTML content to extract the image and its details.
- Downloads the image locally.
- Sends an email with the image as an attachment.
- Reads email configuration from a JSON file.

## Getting Started

To run the application, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore NuGet packages and compile the code.
4. Ensure the `appconfig.json` file is correctly configured with your email settings.
5. Run the `AstronomyPictureOfTheDayNotifier` project to start the console application.

## Dependencies

- HtmlAgilityPack: The application uses this package to parse HTML content.
- System.Net.Mail: The application uses this package to send emails.
- System.Text.Json: The application uses this package to read configuration from a JSON file.

## Usage

1. The application will fetch the Astronomy Picture of the Day from NASA's APOD website.
2. It will parse the HTML content to extract the image and its details.
3. The image will be downloaded locally.
4. An email will be sent with the image as an attachment using the SMTP settings provided in the `appconfig.json` file.
5. The application will delete the downloaded image after sending the email.

## License

This project is licensed under the MIT License.

## Resources Used

- [NASA APOD](https://apod.nasa.gov/apod/astropix.html)
- [HtmlAgilityPack Documentation](https://html-agility-pack.net/documentation)
- [System.Net.Mail Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail)
- [System.Text.Json Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.text.json)
- [Mail trap](https://mailtrap.io/) for the SMTP host and email testing.
- GitHub Copilot to generate code snippets.
