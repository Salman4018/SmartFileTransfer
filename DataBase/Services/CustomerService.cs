using DataBase.Models;
using DataBase.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataBase.Services
{
    public class CustomerService: IGenericService<Customer>
    {
        private readonly DBContext _dbContext;
        private readonly ILogger _log;
        public CustomerService(DBContext dbContext, ILogger log)
        {
            _dbContext = dbContext;
            _log = log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Customer>> GetAll()
        {
            try
            {
                var customersList = await _dbContext.Customer.ToListAsync();
                return customersList;
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
        public async Task<Customer> GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classObject"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Customer> Add(Customer classObject)
        {
            throw new NotImplementedException();
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
        public async Task<bool> Update(Customer classObject)
        {
            throw new NotImplementedException();
        }
    }
}
