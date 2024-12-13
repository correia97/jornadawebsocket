using Jornada.API.Interfaces;
using Jornada.API.Models;
using Jornada.API.Servicos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jornada.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {
        private readonly INotificarServico _notificarService;

        private readonly ILogger<SimulacaoController> _logger;
        public SimulacaoController(INotificarServico notificarService, ILogger<SimulacaoController> logger)
        {
            _notificarService = notificarService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Simulacao>>> Get()
        {
            _logger.LogInformation("Get Simulação");
            return Ok(new List<Simulacao>());
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<Simulacao>> Get([FromRoute] Guid usuarioId, [FromHeader] string correlationId)
        {
            _logger.LogInformation($"Get Simulação usuarioid: {usuarioId} correlationId: {correlationId}");
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

            _logger.LogInformation($"Post Simulação usuarioid: {simulacao.UsuarioId} correlationId: {correlationId}");
            var notificacao = new Notificacao()
            {
                CorrelationId = Guid.Parse(correlationId),
                UsuarioId = simulacao.UsuarioId,
                Mensagem = "contratacao Efetuada",
                Usuario = "xpto"
            };

            Response.Headers.Append("correlationId", correlationId);
            if (!await _notificarService.PublishToTopicAsync(notificacao))
                return BadRequest();

            return Ok();
        }
    }
}
