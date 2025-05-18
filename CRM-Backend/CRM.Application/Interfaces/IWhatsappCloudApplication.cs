namespace CRM.Application.Interfaces
{
    public interface IWhatsappCloudApplication
    {
        object TextMessage(string message, string number);
        Task<bool> ImageMessage(string url, string number, string message);
        object AudioMessage(string url, string number);
        object VideoMessage(string url, string number);
        object DocumentMessage(string url, string number);
        object LocationMessage(string number);
        object ButtonsMessage(string number);
       
    }
}
