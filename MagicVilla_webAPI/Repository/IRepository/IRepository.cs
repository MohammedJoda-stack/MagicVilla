using System.Linq.Expressions;
using MagicVilla_webAPI.Models;

namespace MagicVilla_webAPI.Repository.IRepository;

public interface IRepository<T> where T : class 
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task<T> GetAsync(Expression<Func<T,bool>> filter = null, bool tracked =true);
    Task CreateAsync(T entity);
    Task RemoveAsync(T entity);
    
    Task SaveAsync();
}