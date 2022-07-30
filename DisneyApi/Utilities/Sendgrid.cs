using SendGrid;
using SendGrid.Helpers.Mail;

namespace DisneyApi.Utilities
{
    public class Sendgrid
    {
        public static async Task<Response> SendEmail(string emailAdress, string userName)
        {
            DotNetEnv.Env.Load();

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var email = Environment.GetEnvironmentVariable("EMAILADRESS");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(email, "Arturo");
            var to = new EmailAddress($"{emailAdress}", $"{userName}");
            var subject = "Disney api - Registration successful!";
            var plainTextContent = "";
            var htmlContent = "Welcome to Disney Api!, to get the token you need to log in.";
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                htmlContent
            );
            
            var response = await client.SendEmailAsync(msg);
            return response;

        }
    }
}
