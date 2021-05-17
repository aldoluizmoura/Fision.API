using FIsionAPI.Business.Models.Enums;
using System;

namespace FIsionAPI.Business.Models
{
    public class MovimentoFinanceiroEntidade : Entity
    {
        public ClasseMovimento Classe { get; set; }
        public decimal? ValorReceber { get; set; }
        public decimal? ValorPago { get; set; }
        public decimal? ValorMensal { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? ValorTotal { get; set; }        
        public SituacaoMovimento Situacao { get; set; }
        public DateTime DtPagamento { get; set; }
        public DateTime DtVencimento { get; set; }
        public string CompetenciaMensalidade { get; set; }
        public string CompetenciaPagamento { get; set; }
        public int? QuantidadeAlunos { get; set; }
        public string Observacao { get; set; }        
        public DateTime DataCadastro { get; set; }        
        public Guid ContratoId { get; set; }
        public Caixa Caixa { get; set; }
        public virtual ContratoFinanceiro Contrato { get; set; }
        public TipoMovimento Tipo { get; set; }        
    }
}
