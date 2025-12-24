using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using FIsionAPI.Business.Models.Validacões;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services;

public class EntidadeService : BaseService, IEntidadeService
{
    private readonly IPessoaRepositoy _pessoaRepository;
    private readonly IEntidadeRepository _entidadeRepository;
    private readonly IContratoFinanceiroRepository _contratoRepository;
    private readonly IEnderecoPessoaRepository _enderecoRepository;
    private readonly IMovimentoFinanceiroRepository _movimentoFinanceiro;
    private readonly IUnitOfWork _unitOfWork;

    public EntidadeService(INotificador notificador,
                           IPessoaRepositoy pessoaRepository,
                           IEnderecoPessoaRepository enderecoRepository,
                           IContratoFinanceiroRepository contratoRepository,
                           IMovimentoFinanceiroRepository movimentoFinanceiro,
                           IEntidadeRepository entidadeRepository,
                           IUnitOfWork unitOfWork) : base(notificador)
    {
        _pessoaRepository = pessoaRepository;
        _entidadeRepository = entidadeRepository;
        _contratoRepository = contratoRepository;
        _enderecoRepository = enderecoRepository;
        _movimentoFinanceiro = movimentoFinanceiro;
        _unitOfWork = unitOfWork;

    }

    public async Task<bool> Adicionar(Entidade entidade)
    {
        if (!ExecutarValidacao(new EntidadeValidation(), entidade)
            || !ExecutarValidacao(new PessoaValidation(), entidade.Pessoa)
            || !ExecutarValidacao(new ContratoFinanceiroValidation(), entidade.Contrato))
        {
            return false;
        }

        var cpfExiste = _pessoaRepository.Buscar(e => e.CPF == entidade.Pessoa.CPF).Result.Any();

        if (cpfExiste)
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
            || !ExecutarValidacao(new ContratoFinanceiroValidation(), entidade.Contrato))
        {
            return false;
        }

        await _entidadeRepository.Atualizar(entidade);
        await _unitOfWork.Commit();

        return true;
    }

    public async Task AtualizarContratoFinanceiro(ContratoFinanceiro contrato)
    {
        if (contrato == null)
        {
            Notificar("Objeto não encontrado");
            return;
        }

        await _contratoRepository.Atualizar(contrato);
        await _unitOfWork.Commit();
    }

    public async Task AtualizarEndereco(EnderecoPessoa endereco)
    {
        if (endereco == null)
        {
            Notificar("Objeto não encontrado");
            return;
        }

        await _enderecoRepository.Atualizar(endereco);
        await _unitOfWork.Commit();
    }

    public async Task AtualizarPessoa(Pessoa pessoa)
    {
        if (pessoa == null)
        {
            Notificar("Objeto não encontrado");
            return;
        }

        await _pessoaRepository.Atualizar(pessoa);
        await _unitOfWork.Commit();
    }

    public async Task<bool> Remover(Guid id)
    {
        var entidadeTask = _entidadeRepository.ObterPorEntidadeId(id);
        var contratoTask = _contratoRepository.ObterContratoEntidade(id);

        await Task.WhenAll(entidadeTask, contratoTask);

        var entidade = await entidadeTask;
        var contrato = await contratoTask;

        if (entidade == null)
        {
            Notificar("Entidade não encontrada!");
            return false;
        }

        if (contrato != null)
        {
            var movimento = await _movimentoFinanceiro.BuscarMovimentosPorContratoId(contrato.Id);

            if (movimento?.Any() == true)
            {
                Notificar("Esta entidade não pode ser removida, pois possui movimentos financeiros");
                return false;
            }
        }

        var pessoa = await _pessoaRepository.ObterEntidadePessoa(entidade.PessoaId);
        EnderecoPessoa endereco = null;

        if (pessoa != null)
        {
            endereco = await _enderecoRepository.ObterEnderecoPorPessoaId(pessoa.Id);
        }

        if (endereco != null)
        {
            await _enderecoRepository.Remover(endereco.Id);
        }

        if (contrato != null)
        {
            await _contratoRepository.Remover(contrato.Id);
        }            

        await _entidadeRepository.Remover(entidade.Id);

        if (pessoa != null)
        {
            await _pessoaRepository.Remover(pessoa.Id);
        }

        await _unitOfWork.Commit();

        return true;
    }

    public async Task<IEnumerable<Entidade>> BuscarPorClasse(ClasseEntidade classe)
    {
        if (classe == ClasseEntidade.Aluno)
        {
            return await _entidadeRepository.ObterPorClasse(ClasseEntidade.Aluno);
        }

        return await _entidadeRepository.ObterPorClasse(ClasseEntidade.Profissional);
    }

    public void Dispose()
    {
        _pessoaRepository?.Dispose();
        _entidadeRepository?.Dispose();
        _contratoRepository?.Dispose();
        _enderecoRepository?.Dispose();
        _unitOfWork?.Dispose();
    }

}
