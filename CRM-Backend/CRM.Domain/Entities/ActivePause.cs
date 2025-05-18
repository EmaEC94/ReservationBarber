namespace CRM.Domain.Entities
{
    public class ActivePause : BaseEntity
    {

        public string? Titulo { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime ScheduledTime { get; set; } // Time when the pause is scheduled
       // Relación con Client
        public int? ClientId { get; set; }
        public Client? Client { get; set; } 

        public virtual ICollection<Notification> Notifications { get; set; } = null!;

    }
}
