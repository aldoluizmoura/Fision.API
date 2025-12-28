using FIsionAPI.Business.Models.Enums;
using System;
using System.Collections.Generic;

namespace FIsionAPI.Business.Models;

public class ContratoFinanceiro : Entity
{
    public TipoContrato TipoContrato { get; set; }
    public decimal? ValorMensal { get; set; }
    public decimal? ValorUnitario { get; set; }
    public string Vencimento { get; set; }
    public int Quantidade { get; set; }
    public decimal? Desconto { get; set; }
    public decimal? MargemLucro { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public IEnumerable<MovimentoFinanceiroEntidade> Movimentos { get; set; }
    public virtual Entidade Entidade { get; set; }
    public Guid EntidadeId { get; set; }
}
