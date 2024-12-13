using Jornada.API.Models;

namespace Jornada.API.Interfaces
{
    public interface INotificarServico
    {
        Task<bool> PublishToTopicAsync(Notificacao notificacao);
        Task Setup();
    }
}
