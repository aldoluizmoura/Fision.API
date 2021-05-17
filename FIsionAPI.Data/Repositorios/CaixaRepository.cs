using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios
{
    public class CaixaRepository : Repository<Caixa>, ICaixaRepository
    {
        public CaixaRepository(FisionContext contexto) : base(contexto) { }

        public async Task<Caixa> ObterCaixaPorCompetencia(string competencia)
        {
            return await Db.Caixas.AsNoTracking().FirstOrDefaultAsync(x => x.Competencia == competencia);
        }
        public async Task<Caixa> ObterCaixaPorId(Guid id)
        {
            return await Db.Caixas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<IEnumerable<Caixa>> ObterCaixas()
        {
            return await Db.Caixas.AsNoTracking().ToListAsync();
        }
    }
}
