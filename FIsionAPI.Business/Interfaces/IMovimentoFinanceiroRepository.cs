using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IMovimentoFinanceiroRepository : IRepository<MovimentoFinanceiroEntidade>, IDisposable
{
    Task<MovimentoFinanceiroEntidade> BuscarMovimentoPorId(Guid id);
    Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentos();
    Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentosPorTipo(TipoMovimento CodTipo);
    Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentoPorClasse(ClasseMovimento CodClasse);
    Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentosPorContratoId(Guid ContratoId);
    Task<MovimentoFinanceiroEntidade> BuscarMovimentosPorCompetencia(MovimentoFinanceiroEntidade movimento);
    Task<IEnumerable<MovimentoFinanceiroEntidade>> ObterMovimentoPorTipoCompetencia(TipoMovimento tipo, string competencia);
}
