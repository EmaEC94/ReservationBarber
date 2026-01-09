namespace CRM.Application.Dtos.Client.Request
{
    public class ClientRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int DocumentTypeId { get; set; }
        public int CompanyId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool SendNotifications { get; set; }
        public int State { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }
    }


}
