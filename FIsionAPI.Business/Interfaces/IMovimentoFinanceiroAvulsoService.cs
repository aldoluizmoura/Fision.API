using FIsionAPI.Business.Models;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IMovimentoFinanceiroAvulsoService : IDisposable
{
    Task<bool> AdicionarMovimentoFinanceiroAvulso(MovimentoFinanceiroAvulso movimento);
    Task<bool> AtualizarMovimentoFinanceiroAvulso(MovimentoFinanceiroAvulso movimento);
}
