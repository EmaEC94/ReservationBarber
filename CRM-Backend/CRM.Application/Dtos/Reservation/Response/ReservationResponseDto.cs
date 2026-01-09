namespace CRM.Application.Dtos.Reservation.Response
{
    public class ReservationResponseDto
    {
        public int? RervationId { get; set; }
        public string? Title { get; set; }
        public string? Note { get; set; }
        public string? Message { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserBarberId { get; set; }
        public int SubCatalogId { get; set; }
        public int? ClientId { get; set; }
        public short Price { get; set; }
        public string Payment { get; set; }
        public int State { get; set; }
    }
}
