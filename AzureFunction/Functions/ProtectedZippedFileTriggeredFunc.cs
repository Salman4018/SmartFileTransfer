using System;
using System.IO;
using System.Threading.Tasks;
using DataBase.Models;
using DataBase.Services.Interface;
using Ionic.Zip;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public class ProtectedZippedFileTriggeredFunc
    {
        private readonly IGenericService<ProtectedZippedFiles> _protectedZippedFilesService;
        public ProtectedZippedFileTriggeredFunc(IGenericService<ProtectedZippedFiles> protectedZippedFilesService)
        {
            _protectedZippedFilesService = protectedZippedFilesService;
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
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            string folderPath = @"c:\temp\TempZipFile";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filepath = Path.Combine(folderPath, name);
            SaveStreamAsFile( myBlob, filepath);
            var byteFile= _CreateProtectedZipFile(folderPath, filepath);
            await _SaveZipFileToDB(byteFile, filepath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteFile"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task _SaveZipFileToDB(byte[] byteFile, string filePath)
        {
            var fileName = $"{Path.GetFileNameWithoutExtension(filePath)}.zip";
            var zippedFile = new ProtectedZippedFiles
            {
                Name = fileName,
                FileContent = byteFile,
                CreatedDateTime = DateTime.Now
            };
            await _protectedZippedFilesService.Add(zippedFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="path"></param>
        public static void SaveStreamAsFile(Stream inputStream, string path)
        {
            using var outputFileStream = new FileStream(path, FileMode.Create);
            inputStream.CopyTo(outputFileStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private byte[] _CreateProtectedZipFile(string folderPath, string filePath)
        {
            var zippedFile = Path.Combine(folderPath, $"{Path.GetFileNameWithoutExtension(filePath)}.zip");

            if (File.Exists(zippedFile))
            {
                File.Delete(zippedFile);
            }
            var zip = new ZipFile(zippedFile);
            var entry = zip.AddFile(filePath);
            entry.Password = "123456!";
            zip.Save(zippedFile);

            var byteFile = File.ReadAllBytes(zippedFile);

            return byteFile;
        }
    }


}
