using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;

namespace Jornada.Worker.Servicos
{
    public class ProcessarNotificacaoServico : IProcessarNotificacaoServico
    {
        private readonly IRetornoServico _retornoService;

        public ProcessarNotificacaoServico(IRetornoServico retornoService)
        {
            _retornoService = retornoService;
        }

        public async Task<bool> Processar(Notificacao notificacao)
        {
            var result = await _retornoService.DisparRetorno(notificacao);
            return result;
        }
    }
}
