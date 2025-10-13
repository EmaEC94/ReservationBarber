namespace CRM.Domain.Entities
{
    public class Client : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? UserName { get; set; }
        
        public string? Password { get; set; }
        
        public int DocumentTypeId { get; set; }    

        public  int CompanyId { get; set; }

        public string? DocumentNumber { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public bool SendNotifications { get; set; }

        public string? AuthType { get; set; }

        public virtual DocumentType DocumentType { get; set; } = null!;
        public virtual Company Company { get; set; } = null!;

        public virtual ICollection<ActivePause> ActivePauses { get; set; }    = new  List<ActivePause>();
        public virtual ICollection<Notification> Notifications { get; set; } = null!;
        public string? PasswordResetToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }

    }
}
