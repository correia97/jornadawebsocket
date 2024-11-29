using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;

namespace Jornada.Worker.Servicos
{
    public class ProcessarNotificacaoService : IProcessarNotificacaoService
    {
        private readonly IRetornoService _retornoService;

        public ProcessarNotificacaoService(IRetornoService retornoService)
        {
            _retornoService = retornoService;
        }

        public async Task<bool> Processar(NotificacaoModel notificacao)
        {
            var result = await _retornoService.DisparRetorno(notificacao);
            return result;
        }
    }
}
