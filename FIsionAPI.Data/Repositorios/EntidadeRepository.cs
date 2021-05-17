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
    public class EntidadeRepository : Repository<Entidade>, IEntidadeRepository
    {
        public EntidadeRepository(FisionContext contexto) : base (contexto){ }

        public async Task<IEnumerable<Entidade>> ObterPorClasse(ClasseEntidade CodClasse)
        {
            return await Db.Entidades.AsNoTracking().Where(e => e.Classe == CodClasse).ToListAsync();
        }

        public async Task<Entidade> ObterPorEntidadeId(Guid Id)
        {
            return await Db.Entidades.AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);
        }        
    }
}
