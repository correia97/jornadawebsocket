using Jornada.API.Interfaces;
using Jornada.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jornada.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JornadaController : ControllerBase
    {
        private readonly INotificarService _notificarService;
        public JornadaController(INotificarService notificarService)
        {
            _notificarService = notificarService;
        }
        // GET: api/<JornadaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _notificarService.Setup();
            return new string[] { "value1", "value2" };
        }

        // GET api/<JornadaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JornadaController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NotificacaoModel notificacao)
        {
            await _notificarService.PublishToTopicAsync(notificacao);
            return Ok();
        }

        // PUT api/<JornadaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JornadaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
