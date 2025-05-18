namespace CRM.Domain.Entities
{
    public class DocumentType : BaseEntity
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Abbreviation { get; set; }

        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
