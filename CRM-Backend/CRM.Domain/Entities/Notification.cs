namespace CRM.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string? Message { get; set; } 
        public string? ImageUrl { get; set; } 
        public DateTime SentTime { get; set; } 
        public int ClientId { get; set; }
        public int ActivePauseId { get; set; }
        public ActivePause ActivePause { get; set; } = null!;
        public Client Client { get; set; } = null!;
      

    }
}
