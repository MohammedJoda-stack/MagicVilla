using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Repository.IRepository;

namespace MagicVilla_webAPI.Repository;

public interface IVillaNumberRepository: IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}