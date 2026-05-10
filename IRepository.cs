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
}