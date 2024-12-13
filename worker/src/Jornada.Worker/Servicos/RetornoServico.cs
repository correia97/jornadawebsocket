using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;
using System.Net.Http.Json;

namespace Jornada.Worker.Servicos
{
    public class RetornoServico : IRetornoServico
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<RetornoServico> _logger;
        public RetornoServico(IConfiguration configuration, ILogger<RetornoServico> logger)
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
        public async Task<bool> DisparRetorno(Notificacao notificacao)
        {
            try
            {
                _logger.LogInformation($"Notificar usuario{notificacao.UsuarioId} mensagem {notificacao.Mensagem}");
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/notificacao");
                httpRequest.Content = JsonContent.Create(notificacao);
                var response = await _httpClient.SendAsync(httpRequest);
                var result = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notificar usuario{notificacao.UsuarioId} mensagem {notificacao.Mensagem}");
                throw;
            }
        }
    }
}
