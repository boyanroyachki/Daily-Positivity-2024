using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Quartz;

namespace Daily_Positivity.Jobs
{
    public class SendEmailJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var ollamaService = new OllamaService();
                var positiveMessage = await ollamaService.GeneratePositiveSentenceAsync();
                SendEmails(positiveMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Execute method: {ex.Message}");
            }
        }
        public void SendEmails(string message)
        {
            var emails = new List<string> { };
            foreach (var email in emails)
            {
                try
                {
                    SendEmail(email, "Daily Positive Message", message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {email}: {ex.Message}");
                }
            }
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("YOUR NAME", "YOUR EMAIL ADDRESS"));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                var email = Program.Configuration["EmailSettings:Email"];
                var password = Program.Configuration["EmailSettings:Password"];

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(email, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
