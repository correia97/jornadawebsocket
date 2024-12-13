using AWS.Messaging;
using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;

namespace Jornada.Worker
{
    public class NotificacaoMenssagemHandler : IMessageHandler<Notificacao>
    {

        private readonly IConfiguration _configuration;
        private readonly IProcessarNotificacaoServico _notificacaoService;
        private readonly ILogger<NotificacaoMenssagemHandler> _logger;
        public NotificacaoMenssagemHandler(IConfiguration configuration, IProcessarNotificacaoServico notificacaoService, ILogger<NotificacaoMenssagemHandler> logger)
        {
            _configuration = configuration;
            _notificacaoService = notificacaoService;
            _logger = logger;
        }
        public async Task<MessageProcessStatus> HandleAsync(MessageEnvelope<Notificacao> messageEnvelope, CancellationToken token = default)
        {
            // Add business and validation logic here.
            if (messageEnvelope == null)
            {
                _logger.LogError("Envelope Mensagem nula");
                return MessageProcessStatus.Failed();
            }

            if (messageEnvelope.Message == null)
            {

                _logger.LogError("Mensagem nula");
                return MessageProcessStatus.Failed();
            }

            _logger.LogInformation("Mensagem vai ser processada");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (await _notificacaoService.Processar(messageEnvelope.Message))
                // Return success so the framework will delete the message from the queue.
                return MessageProcessStatus.Success();

            return MessageProcessStatus.Failed();
        }
    }
}
