using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Context;
using CRM.Infrastructure.Persistences.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistences.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        private readonly CrmContext _context;
        public ReservationRepository(CrmContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<ReservationHourAvailble>> GetHourAvailble(DateOnly Day, int UserId)
        {

            var dayParameter = new SqlParameter("@Day", Day);
            var userIdParameter = new SqlParameter("@UserId", UserId);
            var result = _context.ExecuteStoredProcedure<ReservationHourAvailble>("spbReservationtoHourAvailble", dayParameter, userIdParameter);

            return result;
        }
    }
}
