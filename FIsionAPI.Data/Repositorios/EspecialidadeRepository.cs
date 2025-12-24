using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios;

public class EspecialidadeRepository : Repository<Especialidades>, IEspecialidadeRepository
{
    public EspecialidadeRepository(FisionContext contexto) : base(contexto) { }

    public async Task<Especialidades> ObterEspecialidadePorId(Guid id)
    {
        return await Db.Especialidades.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Especialidades>> ObterEspecialidades()
    {
        return await Db.Especialidades.AsNoTracking().ToListAsync();
    }
}
