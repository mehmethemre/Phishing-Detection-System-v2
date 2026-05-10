
using System.Collections.Generic;
using System.Threading.Tasks; // Asenkron işlemler için bu satırı ekledik!

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    
    // Bizim eklediğimiz asenkron metotlar
    Task<T> GetByIdAsync(int id); 
    Task UpdateAsync(T entity);
    using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}