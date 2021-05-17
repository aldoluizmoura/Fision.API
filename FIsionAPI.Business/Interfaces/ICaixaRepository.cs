using FIsionAPI.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces
{
    public interface ICaixaRepository : IRepository<Caixa>, IDisposable
    {
        Task<IEnumerable<Caixa>> ObterCaixas();
        Task<Caixa> ObterCaixaPorId(Guid id);
        Task<Caixa> ObterCaixaPorCompetencia(string competencia);

    }
}
