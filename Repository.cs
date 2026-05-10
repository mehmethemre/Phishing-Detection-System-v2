using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Asenkron metotlar (Task) için bunu ekledik

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // --- Senin Mevcut Metotların ---

    public IEnumerable<T> GetAll() => _dbSet.ToList();
    
    public T GetById(int id) => _dbSet.Find(id);
    
    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    // --- Arayüze Söz Verdiğimiz Yeni Asenkron Metotlar ---

    public async Task<T> GetByIdAsync(int id)
    {
        // Veritabanından ID'ye göre kaydı asenkron bulup getirir
        return await _dbSet.FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        // Veritabanındaki kaydı asenkron günceller ve kaydeder
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}