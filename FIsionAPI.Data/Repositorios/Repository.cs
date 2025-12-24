using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    public readonly FisionContext Db;
    public readonly DbSet<TEntity> Dbset;

    public Repository(FisionContext contexto)
    {
        Db = contexto;
        Dbset = contexto.Set<TEntity>();
    }

    public async Task<List<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        return await Dbset.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> ObterPorId(Guid id)
    {
        return await Dbset.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TEntity>> ObterTodos()
    {
        return await Dbset.AsNoTracking().ToListAsync();
    }

    public Task Adicionar(TEntity entity)
    {
        Dbset.Add(entity);
        return Task.CompletedTask;
    }

    public Task Atualizar(TEntity entity)
    {
        Dbset.Update(entity);
        return Task.CompletedTask;
    }

    public Task Remover(Guid id)
    {
        Dbset.Remove(new TEntity { Id = id });
        return Task.CompletedTask;
    }

    public async Task<int> SaveChanges()
    {
        return await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}
