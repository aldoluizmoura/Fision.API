using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;

namespace FIsionAPI.Business.Models;

public class Caixa : Entity
{
    public string Competencia { get; set; }
    public decimal? ValorDespesa { get; set; }
    public decimal? ValorReceita { get; set; }
    public decimal? ValorTotal { get; set; }
    public SituacaoCaixa Situacao { get; set; }
    public string Status { get; set; }
    public DateTime DataCadastro { get; set; }
    public IEnumerable<MovimentoFinanceiroEntidade> Movimentos { get; set; }
    public IEnumerable<MovimentoFinanceiroAvulso> MovimentosAvulsos { get; set; }
}
