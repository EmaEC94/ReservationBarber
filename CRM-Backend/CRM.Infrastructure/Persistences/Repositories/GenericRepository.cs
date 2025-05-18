using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Context;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRM.Infrastructure.Persistences.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CrmContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(CrmContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAlltAsync()
        {
            var getAll = await _entity
                        .Where(x => x.State.Equals((int)StateTypes.Active) && x.AuditDeleteUser == null && x.AuditDeleteDate == null).AsNoTracking().ToListAsync();
            return getAll;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            var getById = await _entity.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return getById!;
        }
        public async Task<bool> RegisterAsync(T entity)
        {
            entity.AuditCreateUser = 1;
            entity.AuditCreateDate = DateTime.Now;

            await _context.AddAsync(entity);
            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
        public async Task<bool> EditAsync(T entity)
        {
            entity.AuditUpdateUser = 1;
            entity.AuditUpdateDate = DateTime.Now;

            _context.Update(entity);
            _context.Entry(entity).Property(x => x.AuditCreateUser).IsModified = false;
            _context.Entry(entity).Property(x => x.AuditCreateDate).IsModified = false;

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
        public async Task<bool> RemoveAsync(int id)
        {
            T entity = await GetByIdAsync(id);

            entity!.AuditDeleteUser = 1;
            entity.AuditDeleteDate = DateTime.Now;
            entity.State = 0;

            _context.Update(entity);

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
        public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _entity;
            if (filter != null) query = query.Where(filter);

            return query;
        }

        public IQueryable<T> GetAllQueryable()
        {
            var getAllQuery = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null);
            return getAllQuery;
        }

        public async Task<IEnumerable<T>> GetSelectAsync()
        {
            var getAll = await _entity
                .Where(x => x.State.Equals((int)StateTypes.Active) && x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .AsNoTracking()
                .ToListAsync();

            return getAll;

        }
    }
}
