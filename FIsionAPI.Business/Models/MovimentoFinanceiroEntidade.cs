using FIsionAPI.Business.Models.Enums;
using System;

namespace FIsionAPI.Business.Models;

public class MovimentoFinanceiroEntidade : Entity
{
    public ClasseMovimento Classe { get; set; }
    public decimal? ValorReceber { get; set; }
    public decimal? ValorPago { get; set; }
    public decimal? ValorMensal { get; set; }
    public decimal? Desconto { get; set; }
    public decimal? ValorTotal { get; set; }
    public SituacaoMovimento Situacao { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public DateTime DataVencimento { get; set; }
    public string CompetenciaMensalidade { get; set; }
    public string CompetenciaPagamento { get; set; }
    public int? QuantidadeAlunos { get; set; }
    public string Observacao { get; set; }
    public DateTime DataCadastro { get; set; }
    public Guid ContratoId { get; set; }
    public Caixa Caixa { get; set; }
    public virtual ContratoFinanceiro Contrato { get; set; }
    public TipoMovimento Tipo { get; set; }

    public MovimentoFinanceiroEntidade(
        ClasseMovimento classe,
        decimal? valorReceber,
        decimal? valorPago,
        decimal? valorMensal,
        decimal? desconto,
        SituacaoMovimento situacao,
        DateTime dataVencimento,
        DateTime dataCadastro,
        Guid contratoId,
        TipoMovimento tipo,
        string competenciaMensalidade = "",
        string competenciaPagamento = "",
        int? quantidadeAlunos = null,
        string observacao = "")
    {
        Observacao = observacao;
        Classe = classe;
        ValorReceber = valorReceber;
        ValorPago = valorPago;
        ValorMensal = valorMensal;
        Desconto = desconto;
        Situacao = situacao;
        DataVencimento = dataVencimento;
        CompetenciaMensalidade = competenciaMensalidade;
        CompetenciaPagamento = competenciaPagamento;
        QuantidadeAlunos = quantidadeAlunos;
        Observacao = observacao;
        DataCadastro = dataCadastro;
        ContratoId = contratoId;
        Tipo = tipo;
    }
}
