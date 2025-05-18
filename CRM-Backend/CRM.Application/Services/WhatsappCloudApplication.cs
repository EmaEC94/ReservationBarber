using CRM.Application.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CRM.Application.Services
{
    public class WhatsappCloudApplication : IWhatsappCloudApplication
    {

        public object TextMessage(string message, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "text",
                text = new
                {
                    body = message
                }
            };
        }

        public async Task<bool> ImageMessage(string url, string number, string message)
        {

            var data = new
            {

                messaging_product = "whatsapp",
                to = number,
                type = "image",
                image = new
                {
                    link = url,
                    caption = message

                }

            };

            await Execute(data);


            return await Task.FromResult(true);
           


        }
        public object LocationMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "location",
                location = new
                {
                    latitude = "-12.067079752918158",
                    longitude = "-77.03371847563524",
                    name = "Estadio Nacional del Perú",
                    address = "C. José Díaz s/n, Lima 15046"
                }
            };
        }

        public object VideoMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "video",
                video = new
                {
                    link = url
                }
            };
        }
        public object AudioMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "audio",
                audio = new
                {
                    link = url
                }
            };
        }

        public object ButtonsMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new
                    {
                        text = "Selecciona una opción"
                    },
                    action = new
                    {
                        buttons = new List<object>
                        {
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "01",
                                    title = "Comprar"
                                }
                            },
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "02",
                                    title = "Vender"
                                }
                            }
                        }
                    }
                }
            };
        }

        public object DocumentMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "document",
                document = new
                {
                    link = url
                }
            };
        }

        private async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            using (var content = new ByteArrayContent(byteData))
            {
                string endpoint = "https://graph.facebook.com";
                string phoneNumberId = "114198848201885";
                string accessToken = "EAANNRkupqXsBO4aJwmherhA3s5G6wC8rFNCYGZBOglsZBZBUdZC7ZA1Clo2lgoELItYvuUtZC7Y5vOzCEP837RwepeQVYaBaPRQn6niZBzsj12nHTAZCAvJa5ZAiyWlh80w6eNnXpTkUfKqIKANBY2v9PD7PHghGZAY6cNS2eUWxWipTPnslSlaORJZBi4bX0vqq3irKfI6atAWlmkrGQ8uciColH9tjIvsnRZB4aFEyZC70XMY0ZD";
                string uri = $"{endpoint}/v21.0/{phoneNumberId}/messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }      
    }
}
