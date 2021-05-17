using FIsionAPI.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces
{
    public interface IEnderecoPessoaRepository : IRepository<EnderecoPessoa> 
    {
        Task<IEnumerable<EnderecoPessoa>> ObterEnderecos();
        Task<EnderecoPessoa> ObterEnderecoPorId(Guid Id);
        Task<EnderecoPessoa> ObterEnderecoPorPessoaId(Guid PessoaId);
    }
}
