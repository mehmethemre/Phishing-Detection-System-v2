using System.Collections.Generic;
using System.Linq;
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

    // --- Senkron Metotlar ---
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

    // --- Asenkron Metotlar ---
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
}