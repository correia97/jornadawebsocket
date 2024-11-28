using Jornada.API.Models;

namespace Jornada.API.Interfaces
{
    public interface INotificarService
    {
        Task PublishToTopicAsync(NotificacaoModel notificacao);
    }
}
