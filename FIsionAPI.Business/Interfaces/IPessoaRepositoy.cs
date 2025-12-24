using FIsionAPI.Business.Models;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IPessoaRepositoy : IRepository<Pessoa>
{
    Task<Pessoa> ObterPorPessoaId(Guid id);
    Task<Pessoa> ObterPorCPF(string CPF);
    Task<Pessoa> ObterEntidadePessoa(Guid id);
}
