using FIsionAPI.Business.Models.Enums;
using System;

namespace FIsionAPI.Business.Models
{
    public class MovimentoFinanceiroAvulso : Entity
    {
        public ClasseMovimento Classe { get; set; }
        public decimal ValorTotal { get; set; }
        public SituacaoMovimento Situacao { get; set; }
        public DateTime DtPagamento { get; set; }
        public string CompetenciaMensalidade { get; set; }
        public string Descricao { get; set; }        
        public DateTime DataCadastro { get; set; }
        public Caixa Caixa { get; set; }
        public TipoMovimento Tipo { get; set; }        
    }
}
