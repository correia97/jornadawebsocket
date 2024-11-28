
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Jornada.API.Interfaces;
using Jornada.API.Models;
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
            var request = new PublishRequest
            {
                TopicArn = _configuration.GetValue<string>("sns:arn"),
                Message = JsonSerializer.Serialize(notificacao),
            };

            var response = await _client.PublishAsync(request);

            Console.WriteLine($"Successfully published message ID: {response.MessageId}");
        }
    }
}
