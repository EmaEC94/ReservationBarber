using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistences.Interfaces
{
    public interface IUserException : IGenericRepository<UserException>
    {
        Task<IQueryable<ReservationHourAvailble>> GetUserException(int UserId);
    }
}


