using Jornada.Worker.Models;

namespace Jornada.Worker.Interfaces
{
    public interface IProcessarNotificacaoServico
    {
        Task<bool> Processar(Notificacao notificacao);
    }
}
