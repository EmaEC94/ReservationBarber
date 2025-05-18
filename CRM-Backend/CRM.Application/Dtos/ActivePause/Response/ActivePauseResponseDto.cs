namespace CRM.Application.Dtos.ActivePause.Response
{
    public class ActivePauseResponseDto
    {
        public string? Titulo { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string? Image { get; set; }
        public DateTime? AuditCreateDate { get; set; }

        public int State { get; set; }
        public string? StateAtivePause { get; set; }
    }
}
