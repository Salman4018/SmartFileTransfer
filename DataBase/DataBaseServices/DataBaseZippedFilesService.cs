using DataBase.DataBaseServices.Interface;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataBase.DataBaseServices;

public class DataBaseZippedFilesService: IGenericDataBaseService<ZippedFiles>
{
    private readonly DBContext _dbContext;
    private readonly ILogger _log;
    public DataBaseZippedFilesService(DBContext dbContext, ILogger log)
    {
        _dbContext = dbContext;
        _log = log;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<ZippedFiles>> GetAll()
    {
        try
        {
            var zippedFiles = await _dbContext.ZippedFiles.ToListAsync();
            return zippedFiles;
        }
        catch (Exception ex)
        {
            _log.LogError("Error Occurred in ProtectedZippedFilesService 'GetAll'", ex);
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ZippedFiles> GetById(int id)
    {
        try
        {
            var zippedFiles = await _dbContext.ZippedFiles.ToListAsync();
            return zippedFiles.FirstOrDefault(i => i.Id.Equals(id));
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
    /// <param name="zippedFile"></param>
    /// <returns></returns>
    public async Task<ZippedFiles> Add(ZippedFiles zippedFile)
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
    public async Task<bool> Update(ZippedFiles classObject)
    {
        throw new NotImplementedException();
    }
}