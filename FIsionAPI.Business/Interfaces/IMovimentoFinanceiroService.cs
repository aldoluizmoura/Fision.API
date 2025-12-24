using FIsionAPI.Business.Models;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IMovimentoFinanceiroService : IDisposable
{
    Task<bool> AdicionarMensalidade(MovimentoFinanceiroEntidade movimento);
    Task<bool> AdicionarValorProfissional(MovimentoFinanceiroEntidade movimento);
    Task<bool> QuitarMensalidade(MovimentoFinanceiroEntidade movimento);    
    Task<bool> Desquitar(Guid id);
    Task<bool> Remover(Guid Id);
}
