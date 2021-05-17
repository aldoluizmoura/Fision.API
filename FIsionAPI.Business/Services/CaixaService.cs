using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services
{
    public class CaixaService : BaseService, ICaixaService
    {
        private readonly ICaixaRepository _caixaRepository;
        private readonly IMovimentoFinanceiroAvulsoRepository _movimentoAvulsoRepositoy;
        private readonly IMovimentoFinanceiroRepository _movimentoRepository;
         
        public CaixaService(INotificador notificador, 
                            ICaixaRepository caixaRepository, IMovimentoFinanceiroRepository movimentoRepository,
                            IMovimentoFinanceiroAvulsoRepository movimentoAvulsoRepositoy) : base(notificador)
        {
            _caixaRepository = caixaRepository;
            _movimentoAvulsoRepositoy = movimentoAvulsoRepositoy;
            _movimentoRepository = movimentoRepository;
        }
        public async Task<bool> Adicionar(Caixa caixa)
        {
            if (await _caixaRepository.ObterCaixaPorCompetencia(caixa.Competencia) == null)
            {
                caixa.Situacao = Models.Enums.SituacaoCaixa.Aberto;
                caixa.Status = await VerificarStatus(caixa.Competencia);
                caixa.ValorDespesa = await GerarDespesa(caixa.Competencia);
                caixa.ValorReceita = await GerarReceita(caixa.Competencia);
                caixa.ValorTotal = await  GerarReceita(caixa.Competencia) - await GerarDespesa(caixa.Competencia);
                await _caixaRepository.Adicionar(caixa);

                return true;
            }
            else
            {
                Notificar("Caixa para a competência escolhida já existe!");
                return false;
            }
        }        
        public async Task<bool> Remover(Caixa caixa)
        {
            if (caixa.Situacao == Models.Enums.SituacaoCaixa.Fechado)
            {
                Notificar("Um Caixa fechado não pode ser excluído!");
                return false;
            }
            else
            {
                await _caixaRepository.Remover(caixa.Id);
                return true;
            }
        }
        public async Task<bool> FecharCaixa(Caixa caixa)
        {
            if (caixa.Situacao == Models.Enums.SituacaoCaixa.Fechado)
            {
                Notificar("O caixa já está fechado!");
                return false;
            }
            else
            {
                caixa.Situacao = Models.Enums.SituacaoCaixa.Fechado;
                await _caixaRepository.Atualizar(caixa);
                return true;
            }
        }
        public async Task<bool> ReabrirCaixa(Caixa caixa)
        {
            if (caixa.Situacao == Models.Enums.SituacaoCaixa.Aberto)
            {
                Notificar("O caixa já está aberto!");
                return false;
            }
            else
            {
                caixa.Situacao = Models.Enums.SituacaoCaixa.Aberto;
                caixa.Status = await VerificarStatus(caixa.Competencia);
                caixa.ValorDespesa = await GerarDespesa(caixa.Competencia);
                caixa.ValorReceita = await GerarReceita(caixa.Competencia);
                caixa.ValorTotal = await GerarReceita(caixa.Competencia) - await GerarDespesa(caixa.Competencia);
                await _caixaRepository.Atualizar(caixa);
                return true;
            }
        }
        private async Task<decimal> GerarDespesa(string competencia)
        {
            var listaDespesaCompetencia = await _movimentoAvulsoRepositoy.ObterMovimentoPorTipoCompetencia(Models.Enums.TipoMovimento.Despesa, competencia);
            return listaDespesaCompetencia.Sum(m => m.ValorTotal);
        }
        private async Task<decimal?> GerarReceita(string competencia)
        {
            var listaReceita = await _movimentoRepository.ObterMovimentoPorTipoCompetencia(Models.Enums.TipoMovimento.Receita, competencia);
            decimal? TotalReceita = listaReceita.Sum(v => v.ValorPago);

            var listaReceitaMovAvulsos = await _movimentoAvulsoRepositoy.ObterMovimentoPorTipoCompetencia(Models.Enums.TipoMovimento.Receita, competencia);
            decimal? TotalReceitaAvulsos = listaReceitaMovAvulsos.Sum(v => v.ValorTotal);

            return TotalReceita + TotalReceitaAvulsos;
        }
        private async Task<string> VerificarStatus(string competencia)
        {
            if (await GerarReceita(competencia) >= await GerarDespesa(competencia)) return "Positivo";
            else return "Negativo";
        }
        public void Dispose()
        {
            _caixaRepository.Dispose();
            _movimentoAvulsoRepositoy.Dispose();
            _movimentoRepository.Dispose();
        }

      
    }
}
