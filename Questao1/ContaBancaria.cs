using System.Globalization;

namespace Questao1
{
    class ContaBancaria: IContaBancaria
    {

        public int NumeroConta { get;}
        public string NomeTitular { get; set; }
        public double ValorDeposito { get; set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numeroConta, string nomeTitular, double valorDeposito)
        {
            NumeroConta = numeroConta;
            NomeTitular = nomeTitular;
            ValorDeposito = valorDeposito; 
            Saldo = ValorDeposito != 0 ? ValorDeposito : 0.0;
        }

        public ContaBancaria(int numeroConta, string nomeTitular)
        {
            NumeroConta = numeroConta;
            NomeTitular = nomeTitular;            
        }

        public void Deposito(double quantia)
        {
            Saldo += quantia;
        }

        public void Saque(double quantia)
        {
            Saldo -= quantia + 3.50;
        }

        public override string ToString()
        {
            return $"Conta: {NumeroConta}, Titular: {NomeTitular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}
