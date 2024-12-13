using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;

namespace Jornada.Worker.Servicos
{
    public class ProcessarNotificacaoServico : IProcessarNotificacaoServico
    {
        private readonly IRetornoServico _retornoService;
        private readonly ILogger<ProcessarNotificacaoServico> _logger;

        public ProcessarNotificacaoServico(IRetornoServico retornoService, ILogger<ProcessarNotificacaoServico> logger)
        {
            _retornoService = retornoService;
            _logger = logger;
        }

        public async Task<bool> Processar(Notificacao notificacao)
        {
            _logger.LogInformation($"Notificar usuario{notificacao.UsuarioId} mensagem {notificacao.Mensagem}");
            var result = await _retornoService.DisparRetorno(notificacao);
            return result;
        }
    }
}
