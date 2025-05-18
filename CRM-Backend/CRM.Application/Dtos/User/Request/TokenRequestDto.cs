namespace CRM.Application.Dtos.User.Request
{
    public class TokenRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
