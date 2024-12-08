using Jornada.API.Interfaces;
using Jornada.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jornada.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {
        private readonly INotificarServico _notificarService;
        public SimulacaoController(INotificarServico notificarService)
        {
            _notificarService = notificarService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Simulacao>>> Get()
        {
            return Ok(new List<Simulacao>());
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<Simulacao>> Get([FromRoute] Guid usuarioId, [FromHeader] string correlationId)
        {
            Response.Headers.Append("correlationId", correlationId);

            return Ok(new Simulacao()
            {
                Data = DateTime.Now,
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                Valor = Random.Shared.Next(10000)
            });
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Simulacao simulacao, [FromHeader] string correlationId)
        {
            var notificacao = new Notificacao()
            {
                CorrelationId = Guid.Parse(correlationId),
                UsuarioId = simulacao.UsuarioId,
                Mensagem = "contratacao Efetuada",
                Usuario = "xpto"
            };
            await _notificarService.PublishToTopicAsync(notificacao);

            Response.Headers.Append("correlationId", correlationId);
            return Ok();
        }
    }
}
