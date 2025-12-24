using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios;

public class ContratoFinanceiroRepository : Repository<ContratoFinanceiro>, IContratoFinanceiroRepository
{
    public ContratoFinanceiroRepository(FisionContext contexto):base(contexto)  {  }

    public async Task<ContratoFinanceiro> ObterContratoPorId(Guid id)
    {
        return await Db.Contratos.Include(e=>e.Entidade).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ContratoFinanceiro>> ObterContratoPorTipo(TipoContrato CodTipo)
    {
        return await Db.Contratos.AsNoTracking().Where(c => c.Tipo == CodTipo).ToListAsync();
    }

    public async Task<ContratoFinanceiro> ObterContratoEntidade(Guid EntidadeId)
    {
        return await Db.Contratos.AsNoTracking().FirstOrDefaultAsync(c => c.EntidadeId == EntidadeId);
    }
}
