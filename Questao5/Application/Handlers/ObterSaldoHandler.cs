using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class ObterSaldoHandler : IRequestHandler<SaldoContaRequestQuery, SaldoContaResponse>
    {
        private readonly IContaRepository _contaRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public ObterSaldoHandler(
            IContaRepository contaRepository,
            IMovimentacaoRepository movimentacaoRepository)
        {
            _contaRepository = contaRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<SaldoContaResponse> Handle(
            SaldoContaRequestQuery request,
            CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterContaPorNumeroAsync(request.NumeroConta);

            if (conta == null)
                throw new ContaInvalidaException();

            if (!conta.ativo)
                throw new ContaInativaException();

            var movimentacoes = await _movimentacaoRepository.ObterPorContaAsync(conta.IdContaCorrente);

            decimal credito = 0;
            decimal debito = 0;

            foreach (var mov in movimentacoes)
            {
                if (mov.TipoMovimento == TipoMovimentacao.C.ToString())
                    credito += mov.Valor;
                else
                    debito += mov.Valor;
            }

            return new SaldoContaResponse
            {
                Numero = conta.Numero,
                Nome = conta.Nome,
                Saldo = credito - debito
            };
        }
    }

}
