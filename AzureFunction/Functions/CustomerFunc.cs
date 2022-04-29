using System.Threading.Tasks;
using DataBase.DataBaseServices.Interface;
using DataBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public  class CustomerFunc
    {
        private readonly IGenericDataBaseService<Customer> _customerService;
        public CustomerFunc(IGenericDataBaseService<Customer> customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("WarmUpFunction")]
        public Task<IActionResult> WarmUpFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("WarmUpFunction is Triggered");
            return Task.FromResult<IActionResult>(new OkObjectResult("This is a WarmUpFunction Function call!"));
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
            log.LogInformation("C# HTTP trigger function 'GetCustomerData'");

            var customersList = await _customerService.GetAll();
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
            log.LogInformation("C# HTTP trigger function 'GetCustomerDataById'");
            var customerIdString = req.Query["id"];

            if(string.IsNullOrEmpty(customerIdString)) return new BadRequestObjectResult("Id is null");

            if (int.TryParse(customerIdString, out var customerId))
            {
                var customer = await _customerService.GetById(customerId);
                return new OkObjectResult(customer);
            }

            return new BadRequestObjectResult("Error occurred in Function 'GetCustomerDataById' !!");
        }
    }
}
