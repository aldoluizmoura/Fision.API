using FIsionAPI.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IMovimentoFinanceiroAvulsoRepository : IRepository<MovimentoFinanceiroAvulso>, IDisposable
{
    Task<IEnumerable<MovimentoFinanceiroAvulso>> ObterMovimentos();
    Task<MovimentoFinanceiroAvulso> ObterMovimentoPorId(Guid id);
    Task<IEnumerable<MovimentoFinanceiroAvulso>> ObterMovimentoPorTipoCompetencia(Models.Enums.TipoMovimento tipo, string competencia);
    Task<IEnumerable<MovimentoFinanceiroAvulso>> ObterMovimentoPorCompetencia(string competencia);
}
