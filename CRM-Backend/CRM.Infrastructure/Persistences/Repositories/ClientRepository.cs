using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Context;
using CRM.Infrastructure.Persistences.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistences.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly CrmContext _context;
        public ClientRepository(CrmContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Client?> UserByEmail(string email)
        {
            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email != null &&
                                          x.Email.ToLower() == email.ToLower());
        }

    }
}
