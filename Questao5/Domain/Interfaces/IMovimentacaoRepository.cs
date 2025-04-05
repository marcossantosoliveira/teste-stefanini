
using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces
{
    public interface IMovimentacaoRepository
    {
        public Task AdicionarAsync(Movimentacao movimentacao);
        public Task<IEnumerable<Movimentacao>> ObterPorContaAsync(string contaId);
    }
}
