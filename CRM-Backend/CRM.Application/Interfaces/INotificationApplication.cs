using CRM.Application.Commons.Bases.Response;

namespace CRM.Application.Interfaces
{
    public  interface INotificationApplication
    {
        Task<BaseResponse<bool>> SendNotificationActivePause();

    }
}
