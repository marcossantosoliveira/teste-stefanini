using System.ComponentModel.DataAnnotations;
using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class SaldoContaRequestQuery : IRequest<SaldoContaResponse>
    {
        [Required]
        public long NumeroConta { get; set; }
    }
}
