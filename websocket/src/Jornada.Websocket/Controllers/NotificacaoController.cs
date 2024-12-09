using Jornada.Websocket.Hubs;
using Jornada.Websocket.Interfaces;
using Jornada.Worker.Models;
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
        public NotificacaoController(IHubContext<NotificacaoHub> hubContext, IUsuarioRepo usuarioRepo)
        {
            _hubContext = hubContext;
            _usuarioRepo = usuarioRepo;
        }

        // POST api/<NotificacaoController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Notificacao notificacao)
        {
            var usuario = await _usuarioRepo.RecuperarPorUserId(notificacao.UsuarioId);
            if (usuario != null)
                await _hubContext.Clients.Client(usuario.ConnectionId).SendAsync("ReceiveMessage", notificacao.Usuario, notificacao.Mensagem);
            return Ok();
        }

    }
}
