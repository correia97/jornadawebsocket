using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;
using System.Net.Http.Json;

namespace Jornada.Worker.Servicos
{
    public class RetornoServico : IRetornoServico
    {
        private readonly HttpClient _httpClient;
        public RetornoServico(IConfiguration configuration)
        {

            //var hander = new HttpClientHandler
            //{               
            //};
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetValue<string>("api:url"))
            };
        }
        public async Task<bool> DisparRetorno(Notificacao notificacao)
        {

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/notificacao");
            httpRequest.Content = JsonContent.Create(notificacao);
            var response = await _httpClient.SendAsync(httpRequest);

            return response.IsSuccessStatusCode;
        }
    }
}
