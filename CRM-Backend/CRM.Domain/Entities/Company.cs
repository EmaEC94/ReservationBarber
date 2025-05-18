namespace CRM.Domain.Entities
{
    public class Company: BaseEntity
    {
        
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; } 
        public string? Website { get; set; }        
        public string? Industry { get; set; } 
        
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    }
}
