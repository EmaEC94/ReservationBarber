namespace CRM.Domain.Entities
{
    public partial class Reservation : BaseEntity
    {
        public string? Tittle {  get; set; }
        public string? Note { get; set; }
        public string? Message { get; set; }
        public DateTime Apointment { get; set; }
        public int UserBarberId { get; set; }
        public User userBarber { get; set; }
        // Relación con Client
        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public short Price { get; set; }
        public string Payment { get; set; }

    }
}
