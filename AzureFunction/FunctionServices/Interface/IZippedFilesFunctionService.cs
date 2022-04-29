using System.IO;
using System.Threading.Tasks;

namespace AzureFunctions.FunctionServices.Interface;

public interface IZippedFilesFunctionService
{
    Task CreateAndSaveProtectedZippedFile(Stream blob, string folderPath, string filepath);
}