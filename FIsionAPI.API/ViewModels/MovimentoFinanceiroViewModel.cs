using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.API.ViewModels
{
    public class MovimentoFinanceiroViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public int Classe { get; set; }
        public decimal ValorEmAberto { get; set; }
        public decimal ValorPago { get; set; }
        public int Situacao { get; set; }
        public DateTime DtPagamento { get; set; }
        public DateTime DtVencimento { get; set; }
        public string CompetenciaMensalidade { get; set; }
        public string CompetenciaPagamento { get; set; }
        public Guid ContratoId { get; set; }
        public DateTime DataCadastro { get; set; }        
        public int Tipo { get; set; }
        public Guid? CaixaId { get; set; }
    }
}
