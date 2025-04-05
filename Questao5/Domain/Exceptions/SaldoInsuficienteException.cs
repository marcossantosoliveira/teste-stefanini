namespace Questao5.Domain.Exceptions
{
    public class SaldoInsuficienteException : ContaException
    {
        public SaldoInsuficienteException() : base("Saldo insuficiente", "INSUFFICIENT_FUNDS") { }
    }
}
