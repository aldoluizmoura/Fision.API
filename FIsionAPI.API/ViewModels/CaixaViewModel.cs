using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIsionAPI.API.ViewModels;

public class CaixaViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Competencia { get; set; }
    public decimal ValorDespesa { get; set; }
    public decimal ValorReceita { get; set; }
    public int Situacao { get; set; }
    public string Status { get; set; }
    public DateTime DataCadastro { get; set; }
    public IEnumerable<MovimentoFinanceiroViewModel> Movimentos { get; set; }
}
