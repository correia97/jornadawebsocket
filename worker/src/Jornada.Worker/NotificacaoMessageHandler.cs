using Amazon.SQS;
using AWS.Messaging;
using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;

namespace Jornada.Worker
{
    public class NotificacaoMessageHandler : IMessageHandler<NotificacaoModel>
    {

        private readonly IConfiguration _configuration;
        private readonly IProcessarNotificacaoService _notificacaoService;
        public NotificacaoMessageHandler(IConfiguration configuration, IProcessarNotificacaoService notificacaoService)
        {
            _configuration = configuration;
            _notificacaoService = notificacaoService;
        }
        public async Task<MessageProcessStatus> HandleAsync(MessageEnvelope<NotificacaoModel> messageEnvelope, CancellationToken token = default)
        {
            // Add business and validation logic here.
            if (messageEnvelope == null)
            {
                return MessageProcessStatus.Failed();
            }

            if (messageEnvelope.Message == null)
            {
                return MessageProcessStatus.Failed();
            }

            if (await _notificacaoService.Processar(messageEnvelope.Message))
                // Return success so the framework will delete the message from the queue.
                return MessageProcessStatus.Success();

            return MessageProcessStatus.Failed();
        }
    }
}
