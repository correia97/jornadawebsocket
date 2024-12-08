using Jornada.BFF.Interfaces;
using Jornada.BFF.Models;

namespace Jornada.BFF.Servicos
{
    public class JornadaServico : IJornadaServico
    {
        private readonly HttpClient _httpClient;
        public JornadaServico(IConfiguration configuration)
        {
            //var hander = new HttpClientHandler
            //{               
            //};
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetValue<string>("api:url"))
            };
        }

        public async Task<Simulacao> RecuperarSimulacao(Guid usuarioId, Guid correlationId)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/simulacao/{usuarioId}");
            httpRequest.Headers.Add("correlationId", correlationId.ToString());
            var response = await _httpClient.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Simulacao>();
        }

        public async Task<bool> Contratar(Simulacao simulacao, Guid correlationId)
        {

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/simulacao");
            httpRequest.Headers.Add("correlationId", correlationId.ToString());
            httpRequest.Content = JsonContent.Create(simulacao);
            var response = await _httpClient.SendAsync(httpRequest);

            return response.IsSuccessStatusCode;
        }
    }
}
