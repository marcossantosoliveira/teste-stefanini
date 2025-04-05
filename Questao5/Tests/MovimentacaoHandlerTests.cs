
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Interfaces;
using Xunit;

namespace Questao5.Tests
{
    public class MovimentacaoHandlerTests
    {
        private readonly IContaRepository _contaRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly MovimentacaoHandler _handler;

        public MovimentacaoHandlerTests()
        {
            _contaRepository = Substitute.For<IContaRepository>();
            _movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
            _handler = new MovimentacaoHandler(_contaRepository, _movimentacaoRepository);
        }

        [Fact]
        public async Task Handle_ContaNaoEncontrada_DeveLancarContaInativaException()
        {
            // Arrange
            var request = new MovimentacaoContaRequest
            {
                NumeroConta = 123,
                Valor = 100,
                Tipomovimento = TipoMovimentacao.C
            };

            _contaRepository.ObterContaPorNumeroAsync(123).Returns((Conta)null);

            //Assert
            await Assert.ThrowsAsync<ContaInativaException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ContaInativa_DeveLancarContaInativaException()
        {
            // Arrange
            var request = new MovimentacaoContaRequest
            {
                NumeroConta = 123,
                Valor = 100,
                Tipomovimento = TipoMovimentacao.C
            };

            var contaInativa = new Conta { ativo = false };
            _contaRepository.ObterContaPorNumeroAsync(123).Returns(contaInativa);

            //Assert
            await Assert.ThrowsAsync<ContaInativaException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DebitoComSaldoInsuficiente_DeveLancarSaldoInsuficienteException()
        {
            // Arrange
            var request = new MovimentacaoContaRequest
            {
                NumeroConta = 123,
                Valor = 150,
                Tipomovimento = TipoMovimentacao.D
            };

            var contaAtiva = new Conta { IdContaCorrente = "123m", ativo = true };
            _contaRepository.ObterContaPorNumeroAsync(123).Returns(contaAtiva);

            var movimentacoes = new[]
            {
            new Movimentacao { Valor = 100, TipoMovimento = "C" } 
        };
            _movimentacaoRepository.ObterPorContaAsync("123m").Returns(movimentacoes);

            // Assert
            await Assert.ThrowsAsync<SaldoInsuficienteException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_CreditoValido_DeveAdicionarMovimentacaoERetornarId()
        {
            // Arrange
            var request = new MovimentacaoContaRequest
            {
                NumeroConta = 123,
                Valor = 100,
                Tipomovimento = TipoMovimentacao.C
            };

            var contaAtiva = new Conta { IdContaCorrente = "123m", ativo = true };
            _contaRepository.ObterContaPorNumeroAsync(123).Returns(contaAtiva);

            _movimentacaoRepository.ObterPorContaAsync("123m").Returns(Array.Empty<Movimentacao>());
            _movimentacaoRepository.AdicionarAsync(Arg.Any<Movimentacao>()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result.idMovimento);
            await _movimentacaoRepository.Received(1).AdicionarAsync(Arg.Any<Movimentacao>());
        }

        [Fact]
        public async Task Handle_DebitoValido_DeveAdicionarMovimentacaoERetornarId()
        {
            // Arrange
            var request = new MovimentacaoContaRequest
            {
                NumeroConta = 123,
                Valor = 50,
                Tipomovimento = TipoMovimentacao.D
            };

            var contaAtiva = new Conta { IdContaCorrente = "123m", ativo = true };
            _contaRepository.ObterContaPorNumeroAsync(123).Returns(contaAtiva);

            var movimentacoes = new[]
            {
            new Movimentacao { Valor = 100, TipoMovimento = "C" }
        };
            _movimentacaoRepository.ObterPorContaAsync("123m").Returns(movimentacoes);
            _movimentacaoRepository.AdicionarAsync(Arg.Any<Movimentacao>()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result.idMovimento);
            await _movimentacaoRepository.Received(1).AdicionarAsync(Arg.Is<Movimentacao>(m =>
                m.TipoMovimento == "D" && m.Valor == 50));
        }

        [Fact]
        public async Task Handle_DeveCalcularSaldoCorretamenteAposMovimentacao()
        {
            // Arrange
            var request = new MovimentacaoContaRequest
            {
                NumeroConta = 123,
                Valor = 100,
                Tipomovimento = TipoMovimentacao.C
            };

            var contaAtiva = new Conta { IdContaCorrente = "123m", ativo = true };
            _contaRepository.ObterContaPorNumeroAsync(123).Returns(contaAtiva);
                       
            _movimentacaoRepository.ObterPorContaAsync("123m")
                .Returns(
                    Array.Empty<Movimentacao>(),
                    new[] { new Movimentacao { Valor = 100, TipoMovimento = "C" } }                 );

            _movimentacaoRepository.AdicionarAsync(Arg.Any<Movimentacao>()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result.idMovimento);
            await _movimentacaoRepository.Received(1).ObterPorContaAsync("123m"); 
        }
    }
}
