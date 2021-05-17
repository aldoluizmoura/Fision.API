using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Repositorios
{
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepositoy
    {
        public PessoaRepository(FisionContext contexto): base(contexto) {  }
        public async Task<Pessoa> ObterPorPessoaId(Guid id)
        {
            return await Db.Pessoas.AsNoTracking().FirstOrDefaultAsync(p=>p.Id == id);
        }
        public async Task<Pessoa> ObterPorCPF(string CPF)
        {
            return await Db.Pessoas.AsNoTracking().FirstOrDefaultAsync(p => p.CPF == CPF);
        }

        public async Task<Pessoa> ObterEntidadePessoa(Guid id)
        {
            return await Db.Pessoas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
