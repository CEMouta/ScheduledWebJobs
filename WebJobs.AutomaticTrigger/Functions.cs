using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace WebJobs.AutomaticTrigger
{
    public class Functions
    {
        private readonly IConfiguration _configuration;

        public Functions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Singleton]
        // https://arminreiter.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/
        public void Timer1([TimerTrigger("0 * * * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            var value = _configuration["Value1"];

            Console.WriteLine($"Job executed and the value is {value}");
        }

        [Singleton]
        // https://arminreiter.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/
        public void Timer2([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            var value = _configuration["Value2"];

            Console.WriteLine($"Job executed and the value is {value}");
        }
    }
}
