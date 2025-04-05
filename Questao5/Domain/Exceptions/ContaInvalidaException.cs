namespace Questao5.Domain.Exceptions
{
    public class ContaInvalidaException : ContaException
    {
        public ContaInvalidaException() : base("Conta corrente não cadastrada", "INVALID_ACCOUNT") { }
    }
}
