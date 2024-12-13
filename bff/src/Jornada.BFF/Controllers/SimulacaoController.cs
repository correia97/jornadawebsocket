using Jornada.BFF.Interfaces;
using Jornada.BFF.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jornada.BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {
        private readonly IJornadaServico _jornadaServico;
        private readonly ILogger<SimulacaoController> _logger;
        public SimulacaoController(IJornadaServico jornadaServico, ILogger<SimulacaoController> logger)
        {
            _jornadaServico = jornadaServico;
            _logger = logger;
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<Simulacao>> Get(Guid usuarioId, [FromHeader] Guid correlationId)
        {

            _logger.LogInformation($"Get Simulação usuarioid: {usuarioId} correlationId: {correlationId}");
            var result = await _jornadaServico.RecuperarSimulacao(usuarioId, correlationId);
            Response.Headers.Append("correlationId", correlationId.ToString());
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Simulacao simulacao, [FromHeader] Guid correlationId)
        {
            _logger.LogInformation($"Post Simulação usuarioid: {simulacao.UsuarioId} correlationId: {correlationId}");
            var result = await _jornadaServico.Contratar(simulacao, correlationId);
            Response.Headers.Append("correlationId", correlationId.ToString());
            if (!result)
                return BadRequest();

            return Ok(result);
        }

    }
}
