namespace CRM.Domain.Entities
{
    public partial class User : BaseEntity
    {
        public User()
        {

            UserRoles = new HashSet<UserRole>();
         
        }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? Image { get; set; }

        public string? AuthType { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }




        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
