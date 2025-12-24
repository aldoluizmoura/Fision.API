using FIsionAPI.Business.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FIsionAPI.API.ViewModels;

public class MovimentoFinanceiroViewModel
{
    [Key]
    public Guid Id { get; set; }
    public ClasseMovimento Classe { get; set; }
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
