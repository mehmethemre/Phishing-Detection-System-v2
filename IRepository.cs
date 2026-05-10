using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    // Senkron Metotlar
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    
    // Asenkron Metotlar
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}