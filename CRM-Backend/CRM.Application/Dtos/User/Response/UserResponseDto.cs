using Microsoft.AspNetCore.Http;

namespace CRM.Application.Dtos.User.Response
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        //public string? Password { get; set; }
        public string? Email { get; set; }
        public string? AuthType { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime? AuditCreateDate { get; set; }
        public int State { get; set; }
        public string? StateUser { get; set; }
    }
}
