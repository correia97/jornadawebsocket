using Jornada.BFF.Models;

namespace Jornada.BFF.Interfaces
{
    public interface IJornadaServico
    {
        Task<Simulacao> RecuperarSimulacao(Guid usuarioId, Guid correlationId);
        Task<bool> Contratar(Simulacao simulacao, Guid correlationId);
    }
}
