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
    public class MovimentoFinanceiroRepository : Repository<MovimentoFinanceiroEntidade>, IMovimentoFinanceiroRepository
    {
        public MovimentoFinanceiroRepository(FisionContext contexto):base(contexto) { }

        public async Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentos()
        {
            return await Db.Movimentos.AsNoTracking().ToListAsync();
        }

        public async Task<MovimentoFinanceiroEntidade> BuscarMovimentoPorId(Guid id)
        {
            return await Db.Movimentos.AsNoTracking().Include(c=>c.Contrato.Entidade).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentoPorClasse(ClasseMovimento CodClasse)
        {
            return await Db.Movimentos.AsNoTracking().Where(m => m.Classe == CodClasse).ToListAsync();
        }

        public async Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentosPorContratoId(Guid ContratoId)
        {
            return await Db.Movimentos.AsNoTracking().Where(m => m.ContratoId == ContratoId).ToListAsync();
        }

        public async Task<IEnumerable<MovimentoFinanceiroEntidade>> BuscarMovimentosPorTipo(TipoMovimento CodTipo)
        {
            return await Db.Movimentos.AsNoTracking().Where(m=>m.Tipo == CodTipo).ToListAsync();
        }

        public async Task<IEnumerable<MovimentoFinanceiroEntidade>> ObterMovimentoPorTipoCompetencia(TipoMovimento tipo, string competencia)
        {
            return await Db.Movimentos.AsNoTracking().Where(m => m.Tipo == tipo && m.CompetenciaMensalidade == competencia).ToListAsync();
        }

        public async Task <MovimentoFinanceiroEntidade> BuscarMovimentosPorCompetencia(MovimentoFinanceiroEntidade movimento)
        {
            return await Db.Movimentos.AsNoTracking().FirstOrDefaultAsync(m => m.CompetenciaMensalidade == movimento.CompetenciaMensalidade && m.ContratoId == movimento.ContratoId);
        }
    }
}
