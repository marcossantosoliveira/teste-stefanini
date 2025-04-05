using System.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly IDbConnection _bdConexao;
        public ContaRepository(IDbConnection bdConexao)
        {
            _bdConexao = bdConexao;
        }

        public async Task<Conta> ObterContaPorNumeroAsync(long numeroConta)
        {
            var sql = "SELECT * FROM contacorrente WHERE numero = @numeroConta";
            return await _bdConexao.QueryFirstOrDefaultAsync<Conta>(sql, new { numeroConta });
        }
    }
}
