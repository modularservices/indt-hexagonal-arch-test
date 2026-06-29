namespace Indt.Teste.PropostaService.Domain.Exceptions;

public class TransicaoStatusException(string statusAtual, string novoStatus) :
    Exception($"Transição de status inválida '{statusAtual}' -> '{novoStatus}'. ")
{
}