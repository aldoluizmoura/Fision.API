using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.API.ViewModels
{
    public class MovimentoFinanceiroAvulsoViewModel
    {
        [Key]
        [Required] public int Classe { get; set; }
        [Required] public decimal ValorTotal { get; set; }
        public int Situacao { get; set; }
        public DateTime DtPagamento { get; set; }        
        [Required]public string CompetenciaMensalidade { get; set; }
        [Required] public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        [Required(ErrorMessage ="O Tipo é Obrigatório")]public int Tipo { get; set; }
        public Guid? CaixaId { get; set; }

    }
}
