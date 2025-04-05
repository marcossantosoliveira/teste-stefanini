using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Interfaces;
using Xunit;

namespace Questao5.Tests
{
    public class SaldoHandlerTests
    {
        private readonly IContaRepository _contaRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly ObterSaldoHandler _handler;

        public SaldoHandlerTests()
        {
            _contaRepository = Substitute.For<IContaRepository>();
            _movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            _handler = new ObterSaldoHandler(_contaRepository, _movimentacaoRepository);
        }

        [Fact]
        public async Task Handle_Valida_Conta_Retorna_Saldo_Ok()
        {
            // Arrange
            var account = new Conta { IdContaCorrente = "1212121212", Numero = 123, Nome = "John Doe", ativo = true };
            var movimentacoes = new[]
            {
                new Movimentacao { IdContaCorrente = "1212121212", Valor = 100, TipoMovimento = TipoMovimentacao.C.ToString() },
                new Movimentacao { IdContaCorrente = "1212121212", Valor = 50, TipoMovimento = TipoMovimentacao.D.ToString() }
            };

            _contaRepository.ObterContaPorNumeroAsync(123).Returns(account);
            _movimentacaoRepository.ObterPorContaAsync("1212121212").Returns(movimentacoes);

            var query = new SaldoContaRequestQuery { NumeroConta = 123};

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(123, result.Numero);
            Assert.Equal("John Doe", result.Nome);
            Assert.Equal(50, result.Saldo);
        }

        [Fact]
        public async Task Handle_Nao_Existe_Movimentacoes_Saldo_0()
        {
            // Arrange
            var conta = new Conta { IdContaCorrente = "1233443asas", Numero = 123, Nome = "John Doe", ativo = true };

            _contaRepository.ObterContaPorNumeroAsync(123).Returns(conta);
            _movimentacaoRepository.ObterPorContaAsync("1233443asas").Returns(new Movimentacao[0]);

            var query = new SaldoContaRequestQuery { NumeroConta = 123 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.Saldo);
        }
    }
}
