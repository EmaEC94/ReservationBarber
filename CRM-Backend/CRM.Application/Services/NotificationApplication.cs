using CRM.Application.Commons.Bases.Response;
using CRM.Application.Interfaces;
using CRM.Utilities.Static;

namespace CRM.Application.Services
{
    public class NotificationApplication : INotificationApplication
    {
        private readonly IWhatsappCloudApplication _whatsappCloudApplication;

        public NotificationApplication(IWhatsappCloudApplication whatsappCloudApplication)
        {
            _whatsappCloudApplication = whatsappCloudApplication;
        }

        public async Task<BaseResponse<bool>> SendNotificationActivePause()
        {
            var response = new BaseResponse<bool>();

            try
            {

                //Capturar todos los clientes
                //Validar que tiene activa notificaciones
                //Validar a que categoria de Notificaciones esta Inscrita


                //var data = new
                //{
                //    messaging_product = "whatsapp",
                //    to = "50686415183",
                //    type = "text",
                //    text = new
                //    {
                //        body = "este es un mensaje de prueba"
                //    }
                //};


                var sendNotification = await _whatsappCloudApplication.ImageMessage("https://chevroletcr.com/blog/wp-content/uploads/2024/05/Chevrolet_Blazer_2023_BCmedia-21.jpg", "50686415183", "Hola Mira mi carro");



                response.IsSuccess = true;
                response.Data = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }
    }
}
