using Jornada.BFF.Models;

namespace Jornada.BFF.Servicos
{
    public class JornadaServico
    {
        private readonly HttpClient _httpClient;
        public JornadaServico(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("api:url"));
        }

        public async Task<Simulacao> RecuperarSimulacao(Guid usuarioId)
        {
            var response = await _httpClient.GetAsync("/api/simulacao");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Simulacao>();
        }

        public async Task Contratar(Simulacao simulacao)
        {

            await _httpClient.PostAsJsonAsync("/api/simulacao", simulacao);
        }
    }
}
