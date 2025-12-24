using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IEntidadeRepository : IRepository<Entidade>, IDisposable
{
    Task<Entidade> ObterPorEntidadeId(Guid Id);
    Task<IEnumerable<Entidade>> ObterPorClasse(ClasseEntidade CodClasse);
}
