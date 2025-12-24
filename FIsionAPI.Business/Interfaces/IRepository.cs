using FIsionAPI.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task Adicionar(TEntity entity);
    Task<TEntity> ObterPorId(Guid id);
    Task<List<TEntity>> ObterTodos();
    Task Atualizar(TEntity entity);
    Task Remover(Guid Id);
    Task<List<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
}
