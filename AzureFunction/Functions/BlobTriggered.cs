using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public class BlobTriggered
    {
        private static readonly string AzureWebJobsStorage = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private static readonly string TriggeredBlobContainer = "azurefunctions4018blobtriggered/{name}";
        private static readonly string ProcessedBlobContainer = "azurefunctions4018blobprocessed/{name}";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myBlob"></param>
        /// <param name="name"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("BlobTriggeredFunc")]
        public async Task Run([BlobTrigger("azurefunctions4018blobtriggered/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");


            if(string.IsNullOrEmpty(AzureWebJobsStorage))
            {
                log.LogInformation($"Unable to get AzureWebJobsStorage string !!");
                return;
            }

            var processedBlob = new StreamReader(myBlob);
            await _SaveProcessedStream(log, AzureWebJobsStorage, name, processedBlob.BaseStream);

            await _DeleteTriggeredBlob(log, AzureWebJobsStorage, name);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="azureWebJobsStorage"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static async Task _DeleteTriggeredBlob(ILogger log, string azureWebJobsStorage, string name)
        {
            var triggeredBlobContainerClient =
                new BlobContainerClient(azureWebJobsStorage, TriggeredBlobContainer);

            await triggeredBlobContainerClient.DeleteBlobIfExistsAsync(name);
            log.LogInformation($"Blob Name {name} is deleted successfully.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="azureWebJobsStorage"></param>
        /// <param name="name"></param>
        /// <param name="myBlob"></param>
        /// <returns></returns>
        private static async Task _SaveProcessedStream(ILogger log, string azureWebJobsStorage, string name, Stream myBlob)
        {
            var processedBlobName = $"Processed_{name}";
            var processedBlobContainerClient =
                new BlobContainerClient(azureWebJobsStorage, ProcessedBlobContainer);

            await processedBlobContainerClient.DeleteBlobIfExistsAsync(processedBlobName);

            await processedBlobContainerClient.UploadBlobAsync(processedBlobName, myBlob);
            log.LogInformation($"Blob {processedBlobName} is added successfully.");
        }
    }
}
