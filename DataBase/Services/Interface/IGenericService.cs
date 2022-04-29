namespace DataBase.Services.Interface;

public interface IGenericService<TC> where TC : class
{
    Task<List<TC>> GetAll();
    Task<TC> GetById(int id);
    Task<TC> Add(TC classObject);
    Task<bool> Delete(int id);
    Task<bool> Update(TC classObject);
}