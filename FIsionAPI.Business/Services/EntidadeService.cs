using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using FIsionAPI.Business.Models.Validacões;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services
{
    public class EntidadeService : BaseService, IEntidadeService
    {
        private readonly IPessoaRepositoy _pessoaRepository;
        private readonly IEntidadeRepository _entidadeRepository;
        private readonly IContratoFinanceiroRepository _contratoRepository;
        private readonly IEnderecoPessoaRepository _enderecoRepository;
        private readonly IMovimentoFinanceiroRepository _movimentoFinanceiro;
        public EntidadeService(INotificador notificador,
                               IPessoaRepositoy pessoaRepository,
                               IEnderecoPessoaRepository enderecoRepository,
                               IContratoFinanceiroRepository contratoRepository,
                               IMovimentoFinanceiroRepository movimentoFinanceiro,
                               IEntidadeRepository entidadeRepository):base(notificador)
        {
            _pessoaRepository = pessoaRepository;
            _entidadeRepository = entidadeRepository;
            _contratoRepository = contratoRepository;
            _enderecoRepository = enderecoRepository;
            _movimentoFinanceiro = movimentoFinanceiro;
        }

        public async Task<bool> Adicionar(Entidade entidade)
        {
            if (!ExecutarValidacao(new EntidadeValidation(), entidade)
                || !ExecutarValidacao(new PessoaValidation(), entidade.Pessoa)
                || !ExecutarValidacao(new ContratoFinanceiroValidation(), entidade.Contrato)
                ) return false; 

            if (_pessoaRepository.Buscar(e => e.CPF == entidade.Pessoa.CPF).Result.Any())
                {
                    Notificar("Já existe uma pessoa com esse CPF");
                    return false;
                }

            await _entidadeRepository.Adicionar(entidade);
            return true;
        }        

        public async Task<bool> Atualizar(Entidade entidade)
        {
            if (!ExecutarValidacao(new EntidadeValidation(), entidade)
                || !ExecutarValidacao(new PessoaValidation(), entidade.Pessoa)
                || !ExecutarValidacao(new ContratoFinanceiroValidation(), entidade.Contrato)
                ) return false;

            await _entidadeRepository.Atualizar(entidade);
            return true;
        }

        public async Task AtualizarContratoFinanceiro(ContratoFinanceiro contrato)
        {
            if (contrato.Id == null)
            {
                Notificar("Objeto não encontrato");
                return;
            }
            await _contratoRepository.Atualizar(contrato);
        }

        public async Task AtualizarEndereco(EnderecoPessoa endereco)
        {
            if (endereco.Id == null)
            {
                Notificar("Objeto não encontrato");
                return;
            }
            await _enderecoRepository.Atualizar(endereco);
        }        
        public async Task AtualizarPessoa(Pessoa pessoa)
        {
            if (pessoa.Id == null) 
            {
                Notificar("Objeto não encontrato");
                return;
            }
            await _pessoaRepository.Atualizar(pessoa);
        }   

        public async Task<bool> Remover(Guid id)
        {
            var entidade = await _entidadeRepository.ObterPorEntidadeId(id);

            var contrato = await _contratoRepository.ObterContratoEntidade(id);

            if (_movimentoFinanceiro.BuscarMovimentosPorContratoId(contrato.Id).Result.Any())
            {
                Notificar("Esta entidade não pode ser removida, pois possui movimentos financeiros");
                return false;
            }
            
            var pessoa = await _pessoaRepository.ObterEntidadePessoa(entidade.PessoaId);
            var endereco = await _enderecoRepository.ObterEnderecoPorPessoaId(pessoa.Id);

            if (endereco != null)
            {
                await _enderecoRepository.Remover(endereco.Id);
            }
            if (contrato != null)
            {
                await _contratoRepository.Remover(contrato.Id);
            }

            await _entidadeRepository.Remover(id);

            if (pessoa != null)
            {
                await _pessoaRepository.Remover(pessoa.Id);
            }           

            return true;
        }      
       
        public async Task<IEnumerable<Entidade>> BuscarPorClasse(ClasseEntidade classe)
        {
            if(classe == Models.Enums.ClasseEntidade.Aluno)
            {
                return await _entidadeRepository.ObterPorClasse(Models.Enums.ClasseEntidade.Aluno);                
            }
            else
            {
                return await _entidadeRepository.ObterPorClasse(Models.Enums.ClasseEntidade.Profissional);                
            }
        }
        public void Dispose()
        {
            _pessoaRepository?.Dispose();
            _entidadeRepository?.Dispose();
            _contratoRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }

    }
}
