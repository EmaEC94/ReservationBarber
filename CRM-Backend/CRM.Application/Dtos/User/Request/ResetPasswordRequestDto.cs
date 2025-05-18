namespace CRM.Application.Dtos.User.Request
{
    public class ResetPasswordRequestDto
    {
        public string? Email { get; set; }
        public string? CurrentUrlClient { get; set; }

    }
}
