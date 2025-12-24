using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services;

public class MovimentoFinanceiroService : BaseService, IMovimentoFinanceiroService
{
    private readonly IMovimentoFinanceiroRepository _movimentoRepository;
    private readonly IContratoFinanceiroRepository _contratoRepository;

    public MovimentoFinanceiroService(INotificador notificador,
                                      IContratoFinanceiroRepository contratoRepository,
                                      IMovimentoFinanceiroRepository movimentoRepository) : base(notificador)
    {
        _movimentoRepository = movimentoRepository;
        _contratoRepository = contratoRepository;
    }

    public async Task<bool> AdicionarMensalidade(MovimentoFinanceiroEntidade movimento)
    {
        var contratoFinanceiro = await ObterContrato(movimento);

        if (contratoFinanceiro == null)
        {
            Notificar("Contrato financeiro não encontrado");
            return false;
        }

        if (EntidadeInativa(contratoFinanceiro.Entidade.DataSaida))
        {
            Notificar("Entidade Inativa");
            return false;
        }

        if (!await ExisteCompetencia(movimento))
        {
            Notificar($"A mensalidade para a competência {movimento.CompetenciaMensalidade} já existe!");
            return false;
        }

        var mensalidade = CriarMensalidade(movimento, contratoFinanceiro);

        await _movimentoRepository.Adicionar(mensalidade);
        return true;
    }

    public async Task<bool> AdicionarValorProfissional(MovimentoFinanceiroEntidade movimento)
    {
        var contratoFinanceiro = await ObterContrato(movimento);

        if (contratoFinanceiro == null)
        {
            Notificar("Contrato financeiro não encontrado");
            return false;
        }

        if (EntidadeInativa(contratoFinanceiro.Entidade.DataSaida))
        {
            Notificar("Entidade Inativa");
            return false;
        }

        if (!await ExisteCompetencia(movimento))
        {
            Notificar("Esse Profissional já possui valor para a competência " + movimento.CompetenciaMensalidade);
            return false;
        }

        var profissional = CriarValorProfissional(movimento, contratoFinanceiro);

        await _movimentoRepository.Adicionar(profissional);
        return true;
    }

    public async Task<bool> QuitarMensalidade(MovimentoFinanceiroEntidade movimento)
    {
        if (movimento == null)
        {
            Notificar("Movimento inválido");
            return false;
        }

        if (!VerificarCondicoesQuitacao(movimento))
        {
            return false;
        }

        DefinirQuitacao(movimento);

        await _movimentoRepository.Atualizar(movimento);
        return true;
    }

    public async Task<bool> Desquitar(Guid id)
    {
        var movimento = await _movimentoRepository.BuscarMovimentoPorId(id);

        if (movimento == null)
        {
            Notificar("Movimento não encontrado!");
            return false;
        }

        if (!VerificarCondicoesDesquitacao(movimento))
        {
            Notificar("Não é possível desquitar esse movimento!");
            return false;
        }

        Reabrir(movimento);

        await _movimentoRepository.Atualizar(movimento);
        return true;
    }

    public async Task<bool> Remover(Guid Id)
    {
        var movimento = await _movimentoRepository.ObterPorId(Id);

        if (!VerificarCondicoesExclusao(movimento))
        {
            Notificar("Não é possível excluir esse movimento!");
            return false;
        }

        await _movimentoRepository.Remover(Id);
        return true;
    }

    private string DefinirCompetenciaPagamento()
    {
        return DateTime.Now.ToString("yyyyMM");
    }

    private async Task<bool> ExisteCompetencia(MovimentoFinanceiroEntidade movimento)
    {
        if (await _movimentoRepository.BuscarMovimentosPorCompetencia(movimento) != null)
        {
            Notificar("Movimento para essa competência já existe!");
            return false;
        }

        return true;
    }

    private void Reabrir(MovimentoFinanceiroEntidade movimento)
    {
        movimento.DataPagamento = DateTime.MinValue;
        movimento.ValorReceber = VerificarClasseValorReceber(movimento, movimento.Contrato);
        movimento.ValorPago = default;
        movimento.Situacao = SituacaoMovimento.Aberto;
        movimento.CompetenciaPagamento = string.Empty;
    }

    private string DefinirDataVencimento(MovimentoFinanceiroEntidade movimento, ContratoFinanceiro contratoFinanceiro)
    {
        return contratoFinanceiro.Vencimento + "/" + movimento.CompetenciaMensalidade.Substring(0, 2) + "/" + DateTime.Now.Year;
    }

    private async Task<ContratoFinanceiro> ObterContrato(MovimentoFinanceiroEntidade movimento)
    {
        return await _contratoRepository.ObterContratoPorId(movimento.ContratoId);
    }

    private bool EntidadeInativa(DateTime dataSaida)
    {
        if (dataSaida != DateTime.MinValue)
        {
            return true;
        }

        return false;
    }

    private MovimentoFinanceiroEntidade DefinirQuitacao(MovimentoFinanceiroEntidade movimento)
    {
        movimento.DataPagamento = DateTime.Now;
        movimento.ValorPago = VerificarClasseValorReceber(movimento, movimento.Contrato);
        movimento.ValorReceber = 0;
        movimento.Situacao = SituacaoMovimento.Quitado;
        movimento.CompetenciaPagamento = DefinirCompetenciaPagamento();

        return movimento;
    }

    private MovimentoFinanceiroEntidade CriarValorProfissional(MovimentoFinanceiroEntidade movimento, ContratoFinanceiro contratoFinanceiro)
    {
        var valorReceber = ((contratoFinanceiro.ValorUnitario * contratoFinanceiro.Quantidade) * (contratoFinanceiro.MargemLucro) / 100);
        var valorMensal = contratoFinanceiro.Quantidade * contratoFinanceiro.ValorUnitario;

        var dataVencimento = DateTime.Parse(DefinirDataVencimento(movimento, contratoFinanceiro));

        return new MovimentoFinanceiroEntidade(
            ClasseMovimento.Profissional,
            valorReceber,
            valorPago: 0,
            valorMensal,
            0,
            SituacaoMovimento.Aberto,
            dataVencimento,
            DateTime.Now,
            contratoFinanceiro.Id,
            TipoMovimento.Receita,
            competenciaPagamento: movimento.CompetenciaPagamento,
            quantidadeAlunos: contratoFinanceiro.Quantidade);
    }

    private MovimentoFinanceiroEntidade CriarMensalidade(MovimentoFinanceiroEntidade movimento, ContratoFinanceiro contratoFinanceiro)
    {
        var desconto = contratoFinanceiro.ValorMensal * contratoFinanceiro.Desconto / 100;
        var valorReceber = contratoFinanceiro.ValorMensal - desconto;

        var dataVencimento = DateTime.Parse(DefinirDataVencimento(movimento, contratoFinanceiro));

        return new MovimentoFinanceiroEntidade(
            ClasseMovimento.Aluno,
            valorReceber,
            valorPago: 0,
            contratoFinanceiro.ValorMensal,
            desconto,
            SituacaoMovimento.Aberto,
            dataVencimento,
            DateTime.Now,
            contratoFinanceiro.Id,
            TipoMovimento.Receita,
            competenciaMensalidade: movimento.CompetenciaMensalidade
        );
    }

    public void Dispose()
    {
        _movimentoRepository?.Dispose();
        _contratoRepository?.Dispose();
    }
}
