using Indt.Teste.PropostaService.Application.Models;
using Indt.Teste.PropostaService.Application.Ports.In;
using Indt.Teste.PropostaService.Application.Ports.Out;

namespace Indt.Teste.PropostaService.Application.UseCases
{
    public class ListPropostasUseCase : IListPropostasUseCase
    {
        private readonly IPropostaRepository _repository;

        public ListPropostasUseCase(IPropostaRepository propostaRepository) =>
            _repository = propostaRepository;

        public async Task<List<PropostaDetailsReadModel>> ExecuteAsync() =>
            await _repository.GetAllAsync();
    }
}
