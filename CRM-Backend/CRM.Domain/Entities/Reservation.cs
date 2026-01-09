namespace CRM.Domain.Entities
{
    public partial class Reservation : BaseEntity
    {
        public string? Title {  get; set; }
        public string? Note { get; set; }
        public string? Message { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserBarberId { get; set; }
        public User userBarber { get; set; }
        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public int SubCatalogId { get; set; }
        public SubCatalog SubCatalog { get; set; }
        public short Price { get; set; }
        public string Payment { get; set; }

    }
}
