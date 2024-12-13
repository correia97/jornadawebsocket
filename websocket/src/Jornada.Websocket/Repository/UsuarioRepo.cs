using Jornada.Websocket.Interfaces;
using Jornada.Websocket.Models;
using StackExchange.Redis;
namespace Jornada.Websocket.Repository
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _dataBase;
        private readonly ILogger<UsuarioRepo> _logger;
        public UsuarioRepo(IConfiguration configuration, ILogger<UsuarioRepo> logger)
        {
            _redis = ConnectionMultiplexer.Connect($"{configuration.GetValue<string>("redis:connectionstring")}");
            _dataBase = _redis.GetDatabase();
            _logger = logger;
        }

        public async Task<Usuario> RecuperarPorUserId(Guid idUsuario)
        {
            _logger.LogInformation($"RecuperarPorUserId id {idUsuario}");
            var result = await _dataBase.StringGetAsync(idUsuario.ToString());

            if (!result.HasValue)
                return null;

            return new Usuario { Id = idUsuario, ConnectionId = result.ToString() };

        }

        public async Task<bool> CadastrarAtualizar(Usuario usuario)
        {
            _logger.LogInformation($"CadastrarAtualizar id {usuario.Id}");
            var result = await _dataBase.StringSetAsync(usuario.Id.ToString(), usuario.ConnectionId, TimeSpan.FromMinutes(40));
            return result;

        }
    }
}
