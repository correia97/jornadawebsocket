using Jornada.BFF.Interfaces;
using Jornada.BFF.Models;

namespace Jornada.BFF.Servicos
{
    public class JornadaServico : IJornadaServico
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JornadaServico> _logger;
        public JornadaServico(IConfiguration configuration, ILogger<JornadaServico> logger)
        {
            //var hander = new HttpClientHandler
            //{               
            //};
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetValue<string>("api:url"))
            };
            _logger = logger;
        }

        public async Task<Simulacao> RecuperarSimulacao(Guid usuarioId, Guid correlationId)
        {
            _logger.LogInformation($"Recuperar Simulacao usuarioId {usuarioId} correlationid {correlationId}");
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/simulacao/{usuarioId}");
            httpRequest.Headers.Add("correlationId", correlationId.ToString());
            var response = await _httpClient.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Simulacao>();
        }

        public async Task<bool> Contratar(Simulacao simulacao, Guid correlationId)
        {
            _logger.LogInformation($"Contratar usuarioId {simulacao.UsuarioId} correlationid {correlationId}");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/simulacao");
            httpRequest.Headers.Add("correlationId", correlationId.ToString());
            httpRequest.Content = JsonContent.Create(simulacao);
            var response = await _httpClient.SendAsync(httpRequest);

            return response.IsSuccessStatusCode;
        }
    }
}
