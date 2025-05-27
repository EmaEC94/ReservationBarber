using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Dtos.Reservation.Request
{
    public class ReservationAvaibleRequest
    {
        public DateOnly DaySelected { get; set; }
        public int UserId { get; set; }
    }
}
