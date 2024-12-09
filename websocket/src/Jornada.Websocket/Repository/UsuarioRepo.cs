using Jornada.Websocket.Interfaces;
using Jornada.Websocket.Models;
using StackExchange.Redis;
namespace Jornada.Websocket.Repository
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _dataBase;
        public UsuarioRepo(IConfiguration configuration)
        {
            _redis = ConnectionMultiplexer.Connect($"{configuration.GetValue<string>("redis:connectionstring")}");
            _dataBase = _redis.GetDatabase();
        }

        public async Task<Usuario> RecuperarPorUserId(Guid idUsuario)
        {
            var result = await _dataBase.StringGetAsync(idUsuario.ToString());

            if (!result.HasValue)
                return null;

            return new Usuario { Id = idUsuario, ConnectionId = result.ToString() };

        }

        public async Task<bool> CadastrarAtualizar(Usuario usuario)
        {
            var result = await _dataBase.StringSetAsync(usuario.Id.ToString(), usuario.ConnectionId, TimeSpan.FromMinutes(40));
            return result;

        }
    }
}
