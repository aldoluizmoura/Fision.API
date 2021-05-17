using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios
{
    public class MovimentoFinanceiroAvulsoRepository : Repository<MovimentoFinanceiroAvulso>, IMovimentoFinanceiroAvulsoRepository
    {
        public MovimentoFinanceiroAvulsoRepository(FisionContext contexto) : base(contexto) { }

        public async Task<IEnumerable<MovimentoFinanceiroAvulso>> ObterMovimentoPorCompetencia(string competencia)
        {
            return await Db.MovimentoAvulso.AsNoTracking().Where(m => m.CompetenciaMensalidade == competencia).ToListAsync();
        }

        public async Task<MovimentoFinanceiroAvulso> ObterMovimentoPorId(Guid id)
        {
            return await Db.MovimentoAvulso.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MovimentoFinanceiroAvulso>> ObterMovimentoPorTipoCompetencia(TipoMovimento tipo, string competencia)
        {
            return await Db.MovimentoAvulso.AsNoTracking().Where(m => m.Tipo == tipo && m.CompetenciaMensalidade == competencia).ToListAsync();
        }

        public async Task<IEnumerable<MovimentoFinanceiroAvulso>> ObterMovimentos()
        {
            return await Db.MovimentoAvulso.AsNoTracking().ToListAsync();
        }
    }
}
