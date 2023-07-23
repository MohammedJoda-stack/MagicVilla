using System.Linq.Expressions;
using MagicVilla_webAPI.Models;

namespace MagicVilla_webAPI.Repository;

public interface IRepository
{
    Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null);
    Task<Villa> GetAsync(Expression<Func<Villa,bool>> filter = null, bool tracked =true);
    Task CreateAsync(Villa entity);
    Task RemoveAsync(Villa entity);
    Task SaveAsync();
}