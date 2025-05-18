namespace CRM.Application.Dtos.User.Request
{
    public class ChangePasswordRequestDto
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
    }
}
