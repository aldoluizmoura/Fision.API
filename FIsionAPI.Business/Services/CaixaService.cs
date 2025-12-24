using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services;

public class CaixaService : BaseService, ICaixaService
{
    private readonly ICaixaRepository _caixaRepository;
    private readonly IMovimentoFinanceiroAvulsoRepository _movimentoAvulsoRepositoy;
    private readonly IMovimentoFinanceiroRepository _movimentoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CaixaService(INotificador notificador,
                        ICaixaRepository caixaRepository,
                        IMovimentoFinanceiroRepository movimentoRepository,
                        IMovimentoFinanceiroAvulsoRepository movimentoAvulsoRepositoy,
                        IUnitOfWork unitOfWork) : base(notificador)
    {
        _caixaRepository = caixaRepository;
        _movimentoAvulsoRepositoy = movimentoAvulsoRepositoy;
        _movimentoRepository = movimentoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Adicionar(Caixa caixa)
    {
        var caixaPorCompetencia = await _caixaRepository.ObterCaixaPorCompetencia(caixa.Competencia);

        if (caixaPorCompetencia != null)
        {
            Notificar($"Caixa para a competência {caixa.Competencia} já existe!");
            return false;
        }

        var receitaTotal = await SomarReceitaPorCompetencia(caixa.Competencia);
        var despesaTotal = await SomarDespesasPorCompetencia(caixa.Competencia);

        caixa.Situacao = SituacaoCaixa.Aberto;
        caixa.Status = await VerificarStatus(caixa.Competencia);
        caixa.ValorDespesa = despesaTotal;
        caixa.ValorReceita = receitaTotal;
        caixa.ValorTotal = receitaTotal - despesaTotal;

        await _caixaRepository.Adicionar(caixa);
        await _unitOfWork.Commit();

        return true;
    }

    public async Task<bool> Remover(Caixa caixa)
    {
        if (caixa.Situacao == SituacaoCaixa.Fechado)
        {
            Notificar("Um Caixa fechado não pode ser excluído!");
            return false;
        }

        await _caixaRepository.Remover(caixa.Id);
        await _unitOfWork.Commit();

        return true;
    }

    public async Task<bool> FecharCaixa(Caixa caixa)
    {
        if (caixa.Situacao == SituacaoCaixa.Fechado)
        {
            Notificar("O caixa já está fechado!");
            return false;
        }

        caixa.Situacao = SituacaoCaixa.Fechado;
        await _caixaRepository.Atualizar(caixa);
        await _unitOfWork.Commit();

        return true;
    }

    public async Task<bool> ReabrirCaixa(Caixa caixa)
    {
        if (caixa.Situacao == SituacaoCaixa.Aberto)
        {
            Notificar("O caixa já está aberto!");
            return false;
        }

        var valorTotal = await CalcularValorTotal(caixa.Competencia);

        caixa.Situacao = SituacaoCaixa.Aberto;
        caixa.Status = await VerificarStatus(caixa.Competencia);
        caixa.ValorDespesa = await SomarDespesasPorCompetencia(caixa.Competencia);
        caixa.ValorReceita = await SomarReceitaPorCompetencia(caixa.Competencia);
        caixa.ValorTotal = valorTotal;
        await _caixaRepository.Atualizar(caixa);
        await _unitOfWork.Commit();

        return true;
    }

    private async Task<decimal> CalcularValorTotal(string competencia)
    {
        var receitaTotal = SomarReceitaPorCompetencia(competencia);
        var despesaTotal = SomarDespesasPorCompetencia(competencia);

        await Task.WhenAll(receitaTotal, despesaTotal);

        return receitaTotal.Result - despesaTotal.Result;
    }

    private async Task<decimal> SomarDespesasPorCompetencia(string competencia)
    {
        var listaDespesaCompetencia = await _movimentoAvulsoRepositoy.ObterMovimentoPorTipoCompetencia(TipoMovimento.Despesa, competencia);

        return listaDespesaCompetencia.Sum(m => m.ValorTotal);
    }

    private async Task<decimal> SomarReceitaPorCompetencia(string competencia)
    {
        var receitaTask = _movimentoRepository.ObterMovimentoPorTipoCompetencia(TipoMovimento.Receita, competencia);

        var avulsoTask = _movimentoAvulsoRepositoy.ObterMovimentoPorTipoCompetencia(TipoMovimento.Receita, competencia);

        await Task.WhenAll(receitaTask, avulsoTask);

        var totalReceita = receitaTask.Result?.Sum(v => v.ValorPago ?? 0m) ?? 0m;

        var totalReceitaAvulsos = avulsoTask.Result?.Sum(v => v.ValorTotal) ?? 0m;

        return totalReceita + totalReceitaAvulsos;
    }

    private async Task<StatusCaixa> VerificarStatus(string competencia)
    {
        var despesa = SomarDespesasPorCompetencia(competencia);
        var receita = SomarReceitaPorCompetencia(competencia);

        await Task.WhenAll(despesa, receita);

        if (receita.Result >= despesa.Result)
        {
            return StatusCaixa.Positivo;
        }

        return StatusCaixa.Negativo;
    }

    public void Dispose()
    {
        _caixaRepository.Dispose();
        _movimentoAvulsoRepositoy.Dispose();
        _movimentoRepository.Dispose();
    }
}
