using DataBase.Models;
using DataBase.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataBase.Services;

public class ProtectedZippedFilesService: IGenericService<ProtectedZippedFiles>
{
    private readonly DBContext _dbContext;
    private readonly ILogger _log;
    public ProtectedZippedFilesService(DBContext dbContext, ILogger log)
    {
        _dbContext = dbContext;
        _log = log;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<ProtectedZippedFiles>> GetAll()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ProtectedZippedFiles> GetById(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="zippedFile"></param>
    /// <returns></returns>
    public async Task<ProtectedZippedFiles> Add(ProtectedZippedFiles zippedFile)
    {
        try
        {
            _dbContext.Entry(zippedFile).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
            return zippedFile;
        }
        catch (Exception ex)
        {
            _log.LogError("Error Occurred in CustomerService 'GetAll'", ex);
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="classObject"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Update(ProtectedZippedFiles classObject)
    {
        throw new NotImplementedException();
    }
}