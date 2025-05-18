namespace CRM.Application.Dtos.Reservation.Response
{
    public class ReservationResponseDto
    {
        public int? RervationId { get; set; }
        public string? Tittle { get; set; }
        public string? Note { get; set; }
        public string? Message { get; set; }
        public DateTime Apointment { get; set; }
        public int UserBarberId { get; set; }
        // Relación con Client
        public int? ClientId { get; set; }
        public short Price { get; set; }
        public string Payment { get; set; }
        public int State { get; set; }
    }
}
