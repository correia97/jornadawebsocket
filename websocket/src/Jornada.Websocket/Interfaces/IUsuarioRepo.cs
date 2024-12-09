using Jornada.Websocket.Models;

namespace Jornada.Websocket.Interfaces
{
    public interface IUsuarioRepo
    {
        Task<Usuario> RecuperarPorUserId(Guid idUsuario);

        Task<bool> CadastrarAtualizar(Usuario usuario);
    }
}
