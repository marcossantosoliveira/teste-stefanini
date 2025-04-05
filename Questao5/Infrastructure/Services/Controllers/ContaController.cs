
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Exceptions;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo de uma conta corrente
        /// </summary>
        /// <param name="numeroConta">N�mero da conta corrente</param>
        /// <returns>Saldo atual da conta</returns>
        /// <response code="200">Retorna o saldo atual da conta</response>
        /// <response code="400">Se a conta n�o for v�lida ou estiver inativa</response>
        [HttpGet("{numeroConta}/saldo")]
        [ProducesResponseType(typeof(SaldoContaResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> GetSaldo(long numeroConta)
        {
            try
            {
                var request = new SaldoContaRequestQuery { NumeroConta = numeroConta };
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ContaException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = ex.TipoErro,
                    Detail = ex.Message,
                    Status = 400
                });
            }
        }

        /// <summary>
        /// Realiza uma movimenta��o na conta corrente
        /// </summary>
        /// <param name="request">Dados da movimenta��o</param>
        /// <returns>Resultado da transa��o com novo saldo</returns>
        /// <response code="200">Movimenta��o realizada com sucesso</response>
        /// <response code="400">Se houver algum erro de valida��o</response>
        [HttpPost("movimento")]
        [ProducesResponseType(typeof(MovimentacaoContaResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> PostMovimentacao([FromBody] MovimentacaoContaRequest request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ContaException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = ex.TipoErro,
                    Detail = ex.Message,
                    Status = 400
                });
            }
        }
    }
}