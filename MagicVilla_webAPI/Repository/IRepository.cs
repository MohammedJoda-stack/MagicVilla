using System.Linq.Expressions;
using MagicVilla_webAPI.Models;

namespace MagicVilla_webAPI.Repository;

public interface IRepository
{
    Task<List<Villa>> GetAll(Expression<Func<Villa>> filter = null);
    Task<Villa> Get(Expression<Func<Villa>> filter = null, bool tracked =true);
    Task Create(Villa entity);
    Task Remove(Villa entity);
}