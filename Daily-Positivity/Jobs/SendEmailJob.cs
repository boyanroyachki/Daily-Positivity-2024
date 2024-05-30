using Daily_Positivity.Services;
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
        private void SendEmails(string message)
        {
            var emails = new List<string> {"some_email@gmail.com"}; //Fill the list with the addresses you want to recieve daily messages
            foreach (var email in emails)
            {
                try
                {
                    SendEmail(email, "Today's Positive Message", message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {email}: {ex.Message}");
                }
            }
        }

        private void SendEmail(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("YOUR NAME", Program.Configuration["EmailSettings:Email"]));
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
