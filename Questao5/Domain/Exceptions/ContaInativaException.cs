namespace Questao5.Domain.Exceptions
{
    public class ContaInativaException: ContaException
    {
        public ContaInativaException() : base("Conta corrente não cadastrada", "INVALID_ACCOUNT") { }
    }
}
