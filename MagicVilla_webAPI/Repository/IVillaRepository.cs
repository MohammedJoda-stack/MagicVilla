using System.Linq.Expressions;
using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Repository.IRepository;

namespace MagicVilla_webAPI.Repository;

public interface IVillaRepository : IRepository<Villa>
{
   
    Task<Villa> UpdateAsync(Villa entity);
   
}