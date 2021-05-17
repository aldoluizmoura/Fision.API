using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Services
{
    public class MovimentoFinanceiroService : BaseService, IMovimentoFinanceiroService
    {
        private readonly IMovimentoFinanceiroRepository _movimentoRepository;
        private readonly IContratoFinanceiroRepository _contratoRepository;        
        public MovimentoFinanceiroService(INotificador notificador,                                            
                                            IContratoFinanceiroRepository contratoRepository,                                            
                                            IMovimentoFinanceiroRepository movimentoRepository)
                                             : base(notificador)
        {
            _movimentoRepository = movimentoRepository;
            _contratoRepository = contratoRepository;            
        }

        public async Task<bool> AdicionarMensalidade(MovimentoFinanceiroEntidade movimento)
        {
            
            var contrato = await ObterContrato(movimento);                
            

            if (contrato.Entidade.DtSaida != DateTime.Parse("0001-01-01"))
            {
                Notificar("Entidade Inativa");
                return false;
            }

            if (!await VerificarCompetencia(movimento))
            {
                Notificar("A mensalidade para a competência "+movimento.CompetenciaMensalidade+" já existe!");
                return false;
            }

            MovimentoFinanceiroEntidade mensalidade = new MovimentoFinanceiroEntidade();

            mensalidade.DataCadastro = DateTime.Now;
            mensalidade.ValorMensal = contrato.ValorMensal;
            mensalidade.Desconto = (contrato.ValorMensal * (contrato.Desconto) / 100);
            mensalidade.ValorReceber = (contrato.ValorMensal) - (contrato.ValorMensal * (contrato.Desconto) / 100);
            mensalidade.ValorPago = 0;
            mensalidade.Classe = Models.Enums.ClasseMovimento.Aluno;
            mensalidade.Situacao = Models.Enums.SituacaoMovimento.Aberto;
            mensalidade.Tipo = Models.Enums.TipoMovimento.Receita;
            mensalidade.ContratoId = contrato.Id;
            mensalidade.CompetenciaMensalidade = movimento.CompetenciaMensalidade;
            mensalidade.DtVencimento = DateTime.Parse(await DefinirDataVencimento(movimento));

            await _movimentoRepository.Adicionar(mensalidade);
            return true;

        }
        public async Task<bool> AdicionarValorProfissional(MovimentoFinanceiroEntidade movimento)
        {
            var contrato = await ObterContrato(movimento);

            if (contrato.Entidade.DtSaida != DateTime.Parse("0001-01-01"))
            {
                Notificar("Entidade Inativa");
                return false;
            }

            if (!await VerificarCompetencia(movimento))
            {
                Notificar("Esse Profissional já possui valor para a competência " + movimento.CompetenciaMensalidade);
                return false;
            }

            MovimentoFinanceiroEntidade valorProfissional = new MovimentoFinanceiroEntidade();
            valorProfissional.DataCadastro = DateTime.Now;
            valorProfissional.ValorMensal = contrato.Quantidade * contrato.ValorUnitario;
            valorProfissional.ValorPago = 0;            
            valorProfissional.ValorReceber = ((contrato.ValorUnitario * contrato.Quantidade) * (contrato.MargemLucro) / 100);
            valorProfissional.Situacao = Models.Enums.SituacaoMovimento.Aberto;
            valorProfissional.Classe = Models.Enums.ClasseMovimento.Profissional;
            valorProfissional.Tipo = Models.Enums.TipoMovimento.Receita;
            valorProfissional.ContratoId = contrato.Id;
            valorProfissional.DtVencimento = DateTime.Parse(await DefinirDataVencimento(movimento));

            await _movimentoRepository.Adicionar(movimento);
            return true;
        }
        public async Task<bool> Quitar(Guid id)
        {
            var movimento = await _movimentoRepository.BuscarMovimentoPorId(id);

            if (!VerificarCondicoesQuitacao(movimento)) return false;
            else
            {
                if (movimento.Contrato.Entidade.Classe == Models.Enums.ClasseEntidade.Aluno)
                {
                    movimento.DtPagamento = DateTime.Now;
                    movimento.ValorPago = VerificarClasseValorReceber(movimento, movimento.Contrato);
                    movimento.ValorReceber = 0;
                    movimento.Situacao = Models.Enums.SituacaoMovimento.Quitado;
                    movimento.CompetenciaPagamento = DefinirCompetenciaPagamento();

                    await _movimentoRepository.Atualizar(movimento);
                    return true;
                }
                else if (movimento.Contrato.Entidade.Classe == Models.Enums.ClasseEntidade.Profissional)
                {
                    movimento.DtPagamento = DateTime.Now;                    
                    movimento.ValorPago = VerificarClasseValorReceber(movimento, movimento.Contrato);
                    movimento.ValorReceber = 0;
                    movimento.Situacao = Models.Enums.SituacaoMovimento.Quitado;
                    movimento.CompetenciaPagamento = DefinirCompetenciaPagamento();

                    await _movimentoRepository.Atualizar(movimento);
                    return true;
                }
                else
                {
                    Notificar("Classe não permitida no processo de quitação");
                    return false;
                }                
            }

        }
        public async Task<bool> desquitar(Guid id)
        {
            var movimento = await _movimentoRepository.BuscarMovimentoPorId(id);            

            if (!VerificarCondicoesDesquitacao(movimento)) return false;
            else
            {
                if (movimento.Contrato.Entidade.Classe == Models.Enums.ClasseEntidade.Aluno) 
                {
                    movimento.DtPagamento = DateTime.Parse("0001-01-01");
                    movimento.ValorReceber = VerificarClasseValorReceber(movimento, movimento.Contrato);
                    movimento.ValorPago = 0;
                    movimento.Situacao = Models.Enums.SituacaoMovimento.Aberto;
                    movimento.CompetenciaPagamento = null;

                    await _movimentoRepository.Atualizar(movimento);
                    return true;
                }
                else if (movimento.Contrato.Entidade.Classe == Models.Enums.ClasseEntidade.Profissional)
                {
                    movimento.DtPagamento = DateTime.Parse("0001-01-01");
                    movimento.ValorPago = 0;
                    movimento.ValorReceber = VerificarClasseValorReceber(movimento, movimento.Contrato);
                    movimento.Situacao = Models.Enums.SituacaoMovimento.Aberto;
                    movimento.CompetenciaPagamento = null;

                    await _movimentoRepository.Atualizar(movimento);
                    return true;
                }
                else
                {
                    Notificar("Classe não permitida no processo de desquitação");
                    return false;
                }
            }

        }
        public async Task<bool> Remover(Guid Id)
        {
            var movimento = await _movimentoRepository.ObterPorId(Id);
            if (VerificarCondicoesExclusao(movimento))
            {
                await _movimentoRepository.Remover(Id);
                return true;
            }
            else return false;
        }
        private string DefinirCompetenciaPagamento()
        {
            return (DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString()).PadLeft(6, '0');
        }
        private async Task<bool> VerificarCompetencia(MovimentoFinanceiroEntidade movimento)
        {
            if (await _movimentoRepository.BuscarMovimentosPorCompetencia(movimento) != null)
                return false;
            return true;
        }
        private async Task<string> DefinirDataVencimento(MovimentoFinanceiroEntidade movimento)
        {
            var contrato = await ObterContrato(movimento);
            return contrato.Vencimento + "/" + movimento.CompetenciaMensalidade.Substring(0, 2) + "/" + DateTime.Now.Year;
        }
        private async Task<ContratoFinanceiro> ObterContrato(MovimentoFinanceiroEntidade movimento)
        {
            return await _contratoRepository.ObterContratoPorId(movimento.ContratoId);
        }
        public void Dispose()
        {
            _movimentoRepository?.Dispose();
            _contratoRepository?.Dispose();
        }

        
    }
}
