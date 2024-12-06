using Jornada.Websocket.Hubs;
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
        public NotificacaoController(IHubContext<NotificacaoHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // POST api/<NotificacaoController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Notificacao notificacao)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", notificacao.Usuario, notificacao.Mensagem);
            return Ok();
        }

    }
}
