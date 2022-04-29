using System;
using System.IO;
using System.Threading.Tasks;
using AzureFunctions.FunctionServices.Interface;
using DataBase.DataBaseServices.Interface;
using DataBase.Models;
using Ionic.Zip;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public class ZippedFilesFunc
    {
        private readonly IGenericDataBaseService<ProtectedZippedFiles> _protectedZippedFilesService;
        private readonly IZippedFilesFunctionService _protectedZippedFilesFunctionService;
        private ILogger _log;
        public ZippedFilesFunc(
            IGenericDataBaseService<ProtectedZippedFiles> protectedZippedFilesService, 
            IZippedFilesFunctionService protectedZippedFilesFunctionService)
        {
            _protectedZippedFilesService = protectedZippedFilesService;
            _protectedZippedFilesFunctionService = protectedZippedFilesFunctionService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myBlob"></param>
        /// <param name="name"></param>
        /// <param name="log"></param>
        [FunctionName("ProtectedZippedFileTriggeredFunc")]
        public async Task Run([BlobTrigger("azurefunctions4018zipblobtriggered/{name}", Connection = "")]Stream myBlob, string name, ILogger log)
        {
            _log = log;
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            string folderPath = @"c:\temp\TempZipFile";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filepath = Path.Combine(folderPath, name);
             await _protectedZippedFilesFunctionService.CreateAndSaveProtectedZippedFile(myBlob, folderPath, filepath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("GetZippedFiles")]
        public async Task<IActionResult> GetZippedFiles(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function 'GetZippedFiles'");

            var zippedFiles = await _protectedZippedFilesService.GetAll();
            return new OkObjectResult(zippedFiles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("GetZippedFileById")]
        public async Task<IActionResult> GetZippedFileById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function 'GetZippedFileById'");
            var fileId = req.Query["id"];

            if (string.IsNullOrEmpty(fileId)) return new BadRequestObjectResult("Id is null");

            if (int.TryParse(fileId, out var customerId))
            {
                var zippedFile = await _protectedZippedFilesService.GetById(customerId);
                return new OkObjectResult(zippedFile);
            }

            return new BadRequestObjectResult("Error occurred in Function 'GetZippedFileById' !!");
        }

    }


}
