using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class UserException : BaseEntity
    {
        // Barbero al que le aplica la excepción
        public int UserBarberId { get; set; }
        public User UserBarber { get; set; } = null!;

        // Si es un día completo NO disponible
        public bool IsFullDay { get; set; }

        // Inicio del bloqueo
        public DateTime StartDate { get; set; }

        // Fin del bloqueo
        public DateTime EndDate { get; set; }

        // Motivo del bloqueo
        public string Reason { get; set; } = string.Empty;

        // Comentario adicional
        public string? Note { get; set; }
    }
}
