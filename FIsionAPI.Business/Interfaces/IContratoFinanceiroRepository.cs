using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IContratoFinanceiroRepository : IRepository<ContratoFinanceiro>, IDisposable
{
    Task<ContratoFinanceiro> ObterContratoPorId(Guid id);
    Task<IEnumerable<ContratoFinanceiro>> ObterContratoPorTipo(TipoContrato CodTipo);
    Task<ContratoFinanceiro> ObterContratoEntidade(Guid EntidadeId);
}
