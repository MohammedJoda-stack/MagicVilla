using MagicVilla_webAPI.Data;
using MagicVilla_webAPI.Models;

namespace MagicVilla_webAPI.Repository;

public class VillaNumberRepository :Repository<VillaNumber>,IVillaNumberRepository
{
    private readonly ApplicationDbContext db;
    public VillaNumberRepository(ApplicationDbContext _db) : base(_db)
    {
        db = _db;
    }

    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
       
        db.VillaNumbers.Update(entity);
        await db.SaveChangesAsync();
        return entity;
    }
}