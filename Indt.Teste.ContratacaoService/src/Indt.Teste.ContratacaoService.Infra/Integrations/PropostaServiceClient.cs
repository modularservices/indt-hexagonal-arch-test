using Indt.Teste.ContratacaoService.Application.Exceptions;
using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Ports.Out;
using System.Net;
using System.Net.Http.Json;

namespace Indt.Teste.ContratacaoService.Infra.Integrations
{
    public class PropostaServiceClient : IPropostaServiceClient
    {
        private readonly HttpClient _httpClient;

        public PropostaServiceClient(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<PropostaReadModel?> GetByIdAsync(Guid propostaId)
        {
            var response = await _httpClient.GetAsync($"api/propostas/{propostaId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PropostaReadModel>();
        }

        public async Task ContratarPropostaAsync(Guid propostaId)
        {
            var response =
                await _httpClient.PostAsync($"internal/propostas/{propostaId}/contratar", null);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new PropostaNaoEncontradaException(propostaId);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var erro = await response.Content.ReadAsStringAsync();

                throw new Exception($"Erro ao atualizar status: {erro}");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
