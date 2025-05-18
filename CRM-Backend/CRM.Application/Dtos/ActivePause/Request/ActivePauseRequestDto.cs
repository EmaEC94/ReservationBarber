using Microsoft.AspNetCore.Http;

namespace CRM.Application.Dtos.ActivePause.Request
{
    public class ActivePauseRequestDto
    {
        public string? Titulo { get; set; }
        public string? Description { get; set; }
        
        public DateTime ScheduledTime { get; set; }
        public IFormFile? Image { get; set; }
        public int State { get; set; }
    }
}
