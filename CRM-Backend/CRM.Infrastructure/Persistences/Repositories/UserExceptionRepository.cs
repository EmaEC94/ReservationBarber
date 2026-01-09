using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Context;
using CRM.Infrastructure.Persistences.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistences.Repositories
{
    public class UserExceptionRepository :  GenericRepository<UserException>, IUserException
    {
        private readonly CrmContext _context;
    public UserExceptionRepository(CrmContext context) : base(context)
    {
        _context = context;
    }

    public Task<IQueryable<ReservationHourAvailble>> GetUserException(int UserId)
    {
        throw new NotImplementedException();
    }

    }
}
