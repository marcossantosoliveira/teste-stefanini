using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1
{
    public interface IContaBancaria
    {
        public void Deposito(double quantia);

        public void Saque(double quantia);


    }
}
