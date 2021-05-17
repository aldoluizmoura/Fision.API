using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces
{
    public interface IEntidadeService : IDisposable
    {
        Task<bool> Adicionar(Entidade entidade);
        Task<bool> Atualizar(Entidade entidade);
        Task<bool> Remover(Guid id);
        Task<IEnumerable<Entidade>> BuscarPorClasse(ClasseEntidade classe);

        Task AtualizarEndereco(EnderecoPessoa endereco);
        Task AtualizarContratoFinanceiro(ContratoFinanceiro contrato);
        Task AtualizarPessoa(Pessoa pessoa);
    }
}
