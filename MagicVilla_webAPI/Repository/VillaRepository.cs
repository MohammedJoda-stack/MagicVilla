using System.Linq.Expressions;
using MagicVilla_webAPI.Data;
using MagicVilla_webAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_webAPI.Repository;

public class VillaRepository : Repository<Villa>,IVillaRepository
{
    private readonly ApplicationDbContext db;
    
    public VillaRepository( ApplicationDbContext _db) : base(_db)
    {
        db = _db;
    }
  
    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedDate = DateTime.Now;
        db.Villas.Update(entity);
        await db.SaveChangesAsync();
        return entity;
    }

  
}