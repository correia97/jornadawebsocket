
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Jornada.API.Interfaces;
using Jornada.API.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Jornada.API.Servicos
{
    public class NotificarService : INotificarService
    {
        private readonly IAmazonSimpleNotificationService _client;
        private readonly IConfiguration _configuration;
        public NotificarService(IAmazonSimpleNotificationService client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task PublishToTopicAsync(NotificacaoModel notificacao)
        {
            try
            {
                var request = new PublishRequest
                {
                    TopicArn = _configuration.GetValue<string>("sns:arn"),
                    Message = JsonSerializer.Serialize(notificacao),
                };

                var response = await _client.PublishAsync(request);

                Console.WriteLine($"Successfully published message ID: {response.MessageId}");
            }
            catch (Amazon.SimpleNotificationService.Model.InvalidParameterException ex)
            {

                Debug.WriteLine(ex);
                Console.WriteLine(ex);
                Debug.WriteLine(ex.InnerException);
                Console.WriteLine(ex.InnerException);
                if (ex.InnerException is Amazon.Runtime.Internal.HttpErrorResponseException)
                {
                    var response = ((Amazon.Runtime.Internal.HttpErrorResponseException)ex.InnerException);
                   Console.WriteLine( response.Message );
                    Debug.WriteLine(response.Message);
                }

                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
