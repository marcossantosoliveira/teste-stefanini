using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoHandler : IRequestHandler<MovimentacaoContaRequest, MovimentacaoContaResponse>
    {
        private readonly IContaRepository _contaRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public MovimentacaoHandler(
            IContaRepository contaRepository,
            IMovimentacaoRepository movimentacaoRepository)
        {
            _contaRepository = contaRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<MovimentacaoContaResponse> Handle(
            MovimentacaoContaRequest request,
            CancellationToken cancellationToken)
        {

            var conta = await _contaRepository.ObterContaPorNumeroAsync(request.NumeroConta);

            if (conta == null)
                throw new ContaInativaException();

            if (!conta.ativo)
                throw new ContaInativaException();

            if (request.Tipomovimento == TipoMovimentacao.D)
            {
                var movimentacoes = await _movimentacaoRepository.ObterPorContaAsync(conta.IdContaCorrente);
                var saldo = movimentacoes.Sum(t => t.TipoMovimento == TipoMovimentacao.C.ToString() ? t.Valor : -t.Valor);

                if (saldo < request.Valor)
                    throw new SaldoInsuficienteException();
            }

            var movimentacao = new Movimentacao
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = conta.IdContaCorrente,
                Valor = request.Valor,
                TipoMovimento = request.Tipomovimento.ToString(),                
                DataMovimento = DateTime.UtcNow
            };

            await _movimentacaoRepository.AdicionarAsync(movimentacao);
           
            var atualizarMovimentacoes = await _movimentacaoRepository.ObterPorContaAsync(conta.IdContaCorrente);
            var novoSaldo = atualizarMovimentacoes.Sum(t => t.TipoMovimento == TipoMovimentacao.C.ToString() ? t.Valor : -t.Valor);

            return new MovimentacaoContaResponse
            {
                idMovimento = movimentacao.IdMovimento              
            };
        }
    }
}
