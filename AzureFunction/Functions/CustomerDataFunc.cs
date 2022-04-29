using System.Linq;
using System.Threading.Tasks;
using DataBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public  class CustomerDataFunc
    {
        private readonly DBContext _dbContext;
        public CustomerDataFunc(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("WakeUpFunction")]
        public async Task<IActionResult> WakeUpFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            return new OkObjectResult("This is a WakeUp Function call!");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("GetCustomerData")]
        public async Task<IActionResult> GetCustomerData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            var customersList = await _dbContext.Customer.ToListAsync();

            return new OkObjectResult(customersList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("GetCustomerDataById")]
        public async Task<IActionResult> GetCustomerDataById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var customerIdString = req.Query["id"];

            if(string.IsNullOrEmpty(customerIdString)) return new BadRequestObjectResult("Id is null");

            if (int.TryParse(customerIdString, out var customerId))
            {
                var customersList = await _dbContext.Customer.ToListAsync();
                return new OkObjectResult(customersList.FirstOrDefault(i => i.Id.Equals(customerId)));
            }

            return new BadRequestObjectResult("Error occurred in Function 'GetCustomerDataById' !!");
        }
    }
}
