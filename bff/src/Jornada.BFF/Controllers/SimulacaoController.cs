using Jornada.BFF.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jornada.BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Simulacao>>> Get()
        {
            return Ok(new List<Simulacao>());
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<Simulacao>> Get(Guid usuarioId)
        {
            return Ok(new Simulacao());
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Simulacao simulacao)
        {
            return Ok();
        }

    }
}
