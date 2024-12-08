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
        public SimulacaoController(IJornadaServico jornadaServico)
        {
            _jornadaServico = jornadaServico; 
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Simulacao>>> Get()
        {
            return Ok(new List<Simulacao>());
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<Simulacao>> Get(Guid usuarioId, [FromHeader] Guid correlationId)
        {
            var result = await _jornadaServico.RecuperarSimulacao(usuarioId, correlationId);
            Response.Headers.Append("correlationId", correlationId.ToString());
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Simulacao simulacao, [FromHeader] Guid correlationId)
         {
            var result = await _jornadaServico.Contratar(simulacao, correlationId);
            Response.Headers.Append("correlationId", correlationId.ToString());
            return Ok(result);
        }

    }
}
