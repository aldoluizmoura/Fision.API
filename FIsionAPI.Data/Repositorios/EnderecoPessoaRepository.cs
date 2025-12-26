using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios;

public class EnderecoPessoaRepository : Repository<EnderecoPessoa>, IEnderecoPessoaRepository
{
    public EnderecoPessoaRepository(FisionContext contexto):base(contexto) { }

    public async Task<EnderecoPessoa> ObterEnderecoPorId(Guid Id)
    {
        return await Db.Enderecos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);
    }

    public async Task<EnderecoPessoa> ObterEnderecoPorPessoaId(Guid PessoaId)
    {
        return await Db.Enderecos.AsNoTracking().FirstOrDefaultAsync(e => e.PessoaId == PessoaId);
    }

    public async Task<IEnumerable<EnderecoPessoa>> ObterEnderecos()
    {
        return await Db.Enderecos.AsNoTracking().ToListAsync();
    }
}
