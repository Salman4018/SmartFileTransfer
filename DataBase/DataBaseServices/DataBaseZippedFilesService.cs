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
    public async Task<ZippedFiles> GetById(int id)
    {
        try
        {
            var zippedFiles = await _dbContext.ZippedFiles.ToListAsync();
            return zippedFiles.FirstOrDefault(i => i.Id.Equals(id));
        }
        catch (Exception ex)
        {
            _log.LogError("Error Occurred in CustomerService 'GetById'", ex);
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
            _log.LogError("Error Occurred in CustomerService 'Add'", ex);
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> Delete(int id)
    {
        try
        {
            var zippedFile = new ZippedFiles() { Id = id };
            _dbContext.Entry(zippedFile).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError("Error Occurred in CustomerService 'Delete'", ex);
            return false;
        }
    }

 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="zippedFile"></param>
    /// <returns></returns>
    public async Task<bool> Update(ZippedFiles zippedFile)
    {
        try
        {
            _dbContext.Entry(zippedFile).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError("Error Occurred in CustomerService 'Update'", ex);
            return false;
        }
    }
}