using Daily_Positivity.Jobs;
using Microsoft.Extensions.Configuration;
using Quartz.Impl;
using Quartz;

namespace Daily_Positivity
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            try
            {
                IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();

                IJobDetail job = JobBuilder.Create<SendEmailJob>()
                    .WithIdentity("emailJob", "group1")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("emailTrigger", "group1")
                    .StartNow()
                    .WithCronSchedule("0 0 12 * * ?")
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                // Keep the application running
                Console.WriteLine("Press [Enter] to close the application.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }
    }
}