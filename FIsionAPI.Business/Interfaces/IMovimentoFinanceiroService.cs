using FIsionAPI.Business.Models;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces
{
    public interface IMovimentoFinanceiroService : IDisposable
    {
        Task<bool> AdicionarMensalidade(MovimentoFinanceiroEntidade movimento);
        Task<bool> AdicionarValorProfissional(MovimentoFinanceiroEntidade movimento);
        Task<bool> Quitar(Guid id);
        Task<bool> desquitar(Guid id);
        Task<bool> Remover(Guid Id);
    }
}
