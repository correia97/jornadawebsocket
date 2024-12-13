using Jornada.Websocket.Hubs;
using Jornada.Websocket.Interfaces;
using Jornada.Websocket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jornada.Websocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {

        private readonly IHubContext<NotificacaoHub> _hubContext;
        private readonly IUsuarioRepo _usuarioRepo;
        private readonly ILogger<NotificacaoController> _logger;
        public NotificacaoController(IHubContext<NotificacaoHub> hubContext, IUsuarioRepo usuarioRepo, ILogger<NotificacaoController> logger)
        {
            _hubContext = hubContext;
            _usuarioRepo = usuarioRepo;
            _logger = logger;
        }

        // POST api/<NotificacaoController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Notificacao notificacao)
        {
            _logger.LogInformation($"Post Notificar usuarioid {notificacao.UsuarioId} mensagem {notificacao.Mensagem}");
            var usuario = await _usuarioRepo.RecuperarPorUserId(notificacao.UsuarioId);
            if (usuario != null)
                await _hubContext.Clients.Client(usuario.ConnectionId).SendAsync("ReceiveMessage", notificacao.Usuario, notificacao.Mensagem);
            return Ok();
        }

    }
}
