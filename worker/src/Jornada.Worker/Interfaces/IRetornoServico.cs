using Jornada.Worker.Models;

namespace Jornada.Worker.Interfaces
{
    public interface IRetornoServico
    {
        Task<bool> DisparRetorno(Notificacao notificacao);
    }
}
