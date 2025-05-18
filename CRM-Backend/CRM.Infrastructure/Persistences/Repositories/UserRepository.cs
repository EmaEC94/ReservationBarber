using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Context;
using CRM.Infrastructure.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Persistences.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly CrmContext _context;
        public UserRepository(CrmContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> UserByEmail(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email!.Equals(email));
            return user!;
        }
    }
}
