using CRM.Domain.Entities;
using System.Linq.Expressions;

namespace CRM.Infrastructure.Persistences.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAllQueryable();
        Task<IEnumerable<T>> GetAlltAsync();
        Task<IEnumerable<T>> GetSelectAsync();

        Task<T> GetByIdAsync(int id);
        Task<bool> RegisterAsync(T entity);

        Task<bool> EditAsync(T entyity);

        Task<bool> RemoveAsync(int id);

        IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null); //Devolver un Iqueribable en base a entidad que se le pase
    }
}
