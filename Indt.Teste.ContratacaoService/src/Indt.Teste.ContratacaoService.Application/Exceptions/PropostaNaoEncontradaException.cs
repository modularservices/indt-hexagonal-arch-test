namespace Indt.Teste.ContratacaoService.Application.Exceptions;

public class PropostaNaoEncontradaException(Guid propostaId) :
    Exception($"Proposta ID {propostaId} não encontrada.")
{
}
