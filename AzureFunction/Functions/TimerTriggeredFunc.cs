using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;

// Schedule Expression
//0 */ 5 * ***      once every five minutes
//0 0 * * * *	    once at the top of every hour
//0 0 */2 * * *	    once every two hours
//0 0 9-17 * * *	once every hour from 9 AM to 5 PM
//0 30 9 * * *	    at 9:30 AM every day
//0 30 9 * * 1-5	at 9:30 AM every weekday
//0 30 9 * Jan Mon	at 9:30 AM every Monday in January

namespace AzureFunctions.Functions
{
    public class TimerTriggeredFunc
    {
        [FunctionName("TimerTriggeredFunc")]
        public void Run([TimerTrigger("* * */12 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
