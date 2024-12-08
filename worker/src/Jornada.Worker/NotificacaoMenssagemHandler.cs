using AWS.Messaging;
using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;

namespace Jornada.Worker
{
    public class NotificacaoMenssagemHandler : IMessageHandler<Notificacao>
    {

        private readonly IConfiguration _configuration;
        private readonly IProcessarNotificacaoServico _notificacaoService;
        public NotificacaoMenssagemHandler(IConfiguration configuration, IProcessarNotificacaoServico notificacaoService)
        {
            _configuration = configuration;
            _notificacaoService = notificacaoService;
        }
        public async Task<MessageProcessStatus> HandleAsync(MessageEnvelope<Notificacao> messageEnvelope, CancellationToken token = default)
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
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (await _notificacaoService.Processar(messageEnvelope.Message))
                // Return success so the framework will delete the message from the queue.
                return MessageProcessStatus.Success();

            return MessageProcessStatus.Failed();
        }
    }
}
