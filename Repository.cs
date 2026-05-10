using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks; // Asenkron metotlar (Task) için bunu ekledik

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }


    // --- Senin Mevcut Metotların ---

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    
    public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    
    public async Task AddAsync(T entity) 
    { 
        await _dbSet.AddAsync(entity); 
        await _context.SaveChangesAsync(); 
    }
    
    public async Task UpdateAsync(T entity) 
    { 
        _dbSet.Update(entity); 
        await _context.SaveChangesAsync(); 
    }
    
    public async Task DeleteAsync(int id) 
    { 
        var entity = await _dbSet.FindAsync(id); 
        if(entity != null) 
        { 
            _dbSet.Remove(entity); 
            await _context.SaveChangesAsync(); 
        } 
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