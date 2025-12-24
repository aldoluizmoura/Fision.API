using FIsionAPI.Business.Models.Enums;
using System;

namespace FIsionAPI.Business.Models;

public class MovimentoFinanceiroAvulso : Entity
{
    public ClasseMovimento Classe { get; set; }
    public decimal ValorTotal { get; set; }
    public SituacaoMovimento Situacao { get; set; }
    public DateTime DataPagamento { get; set; }
    public string CompetenciaMensalidade { get; set; }
    public string Descricao { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public Caixa Caixa { get; set; }
    public TipoMovimento Tipo { get; set; }

    public bool ValidarMovimentoFinanceiro(out string erro)
    {
        if (string.IsNullOrWhiteSpace(CompetenciaMensalidade))
        {
            erro = "A competência da mensalidade é obrigatória.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Descricao))
        {
            erro = "A descrição do movimento é obrigatória.";
            return false;
        }

        if (ValorTotal < 0)
        {
            erro = "O valor total do movimento não pode ser negativo.";
            return false;
        }

        erro = string.Empty;
        return true;
    }
}
