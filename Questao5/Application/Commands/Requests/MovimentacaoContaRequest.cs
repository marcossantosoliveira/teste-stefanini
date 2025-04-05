using System.ComponentModel.DataAnnotations;
using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoContaRequest : IRequest<MovimentacaoContaResponse>
    {
        public long NumeroConta { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimentacao Tipomovimento { get; set; }

    }
}
