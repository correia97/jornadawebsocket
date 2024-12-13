using Jornada.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jornada.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly INotificarServico _notificarService;

        private readonly ILogger<ConfigController> _logger;
        public ConfigController(INotificarServico notificarService, ILogger<ConfigController> logger)
        {
            _notificarService = notificarService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> ConfigurarAmbiente()
        {
            await _notificarService.Setup();
            return Ok();
        }
    }
}
