namespace Questao5.Domain.Exceptions
{
    public class ContaException : Exception
    {

        public string TipoErro { get; }

        protected ContaException(string mensagem, string tipoErro) : base(mensagem)
        {
            TipoErro = tipoErro;
        }
    }
}
