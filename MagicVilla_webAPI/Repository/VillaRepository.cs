using System.Linq.Expressions;
using MagicVilla_webAPI.Data;
using MagicVilla_webAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_webAPI.Repository;

public class VillaRepository : IRepository
{
    private readonly ApplicationDbContext db;
    
    public VillaRepository( ApplicationDbContext _db)
    {
        db = _db;
    }
    public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
    {
        IQueryable<Villa> query = db.Villas;
        if (filter != null)
        {
            query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
    {
        IQueryable<Villa> query = db.Villas;
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

    public async Task CreateAsync(Villa entity)
    {
        await db.Villas.AddAsync(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(Villa entity)
    {

         db.Villas.Remove(entity);
         await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}