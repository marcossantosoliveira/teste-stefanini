﻿
using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces
{
    public interface IContaRepository
    {
        Task<Conta> ObterContaPorNumeroAsync(long numeroConta);
    }
}
