namespace Questao5.Application.Queries.Responses
{
    public class SaldoContaResponse
    {
        public long Numero{ get; set; }
        public string Nome { get; set; }
        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
        public decimal Saldo { get; set; }
               
    }
}
