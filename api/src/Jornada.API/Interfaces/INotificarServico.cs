using Jornada.API.Models;

namespace Jornada.API.Interfaces
{
    public interface INotificarServico
    {
        Task PublishToTopicAsync(Notificacao notificacao);
        Task Setup();
    }
}
