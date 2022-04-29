using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctions.FunctionServices.Interface;
using DataBase.DataBaseServices.Interface;
using DataBase.Models;
using Ionic.Zip;

namespace AzureFunctions.FunctionServices;

public class ZippedFilesFunctionService : IZippedFilesFunctionService
{
    private readonly IGenericDataBaseService<ZippedFiles> _protectedZippedFilesService;

    public ZippedFilesFunctionService(IGenericDataBaseService<ZippedFiles> protectedZippedFilesService)
    {
        _protectedZippedFilesService = protectedZippedFilesService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="blob"></param>
    /// <param name="folderPath"></param>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public async Task CreateAndSaveProtectedZippedFile(Stream blob, string folderPath, string filepath)
    {

        SaveStreamAsFile(blob, filepath);
        var byteFile = _CreateProtectedZipFile(folderPath, filepath);

        var fileName = $"{Path.GetFileNameWithoutExtension(filepath)}.zip";
        var zippedFile = new ZippedFiles
        {
            Name = fileName,
            FileContent = byteFile,
            CreatedDateTime = DateTime.Now
        };


        var files = await _protectedZippedFilesService.GetAll();
        var existingFile = files.FirstOrDefault(f =>
            f.Name.ToUpper().Equals(fileName.ToUpper()));


        if (existingFile is not null)
        {
            existingFile.FileContent = byteFile;
            existingFile.CreatedDateTime = DateTime.Now;
            await _protectedZippedFilesService.Update(existingFile);
            return;
        }
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