using System.Linq.Expressions;
using MagicVilla_webAPI.Data;
using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_webAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext db;
    internal DbSet<T> dbSet;

    public Repository( ApplicationDbContext _db)
    {
        db = _db;
        this.dbSet = db.Set<T>();
    }
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
    {
        IQueryable<T> query = dbSet;
        if (!tracked)
        {
            query.AsNoTracking();
        }
        if (filter != null)
        {
            query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();

    }

    public async Task CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(T entity)
    {

        dbSet.Remove(entity);
        await SaveAsync();
    }

    
    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }
    
}