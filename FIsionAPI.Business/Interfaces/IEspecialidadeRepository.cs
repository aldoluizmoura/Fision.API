using FIsionAPI.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces
{
    public interface IEspecialidadeRepository : IRepository<Especialidades>, IDisposable
    {
        Task<IEnumerable<Especialidades>> ObterEspecialidades();
        Task<Especialidades> ObterEspecialidadePorId(Guid id);
    }
}
