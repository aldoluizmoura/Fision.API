using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.API.ViewModels
{
    public class ContratoFinanceiroViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public int Tipo { get; set; }
        public decimal ValorMensal { get; set; }
        public string Vencimento { get; set; }
        public int? Quantidade { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? MargemLucro { get; set; }
        public EntidadeViewModel Entidade { get; set; }
        public Guid EntidadeId { get; set; }
        public IEnumerable<MovimentoFinanceiroViewModel> Movimentos { get; set; }
    }
}
