using System.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;

namespace Questao5.Infrastructure.Database.CommandStore.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly IDbConnection _bdConexao;

        public MovimentacaoRepository(IDbConnection bdConexao)
        {
            _bdConexao = bdConexao;
        }
       
        public async Task AdicionarAsync(Movimentacao movimentacao)
        {
            
            var sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, valor, tipomovimento, datamovimento)
                        VALUES (@IdMovimento, @IdContaCorrente, @Valor, @TipoMovimento, @DataMovimento);
                        SELECT idmovimento FROM movimento WHERE idmovimento = @IdMovimento;"

            ;
            movimentacao.IdMovimento = await _bdConexao.ExecuteScalarAsync<string>(sql, movimentacao);
        }
              

        public async Task<IEnumerable<Movimentacao>> ObterPorContaAsync(string contaId)
        {
            var sql = "SELECT * FROM movimento WHERE idcontacorrente = @contaId ORDER BY datamovimento";
            return await _bdConexao.QueryAsync<Movimentacao>(sql, new { contaId = contaId });
        }
    }
}
