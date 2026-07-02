using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Enums;
using System;

namespace FIsionAPI.Tests.Helpers;

public static class TestDataHelper
{
    public const string CpfValido = "11144477735";

    public static Entidade CriarEntidadeAlunoValida()
    {
        return new Entidade
        {
            Classe = ClasseEntidade.Aluno,
            Pessoa = new Pessoa
            {
                CPF = CpfValido,
                Nome = "João Teste",
                DataNascimento = new DateTime(1990, 1, 1),
                Sexo = "M",
                Endereco = CriarEnderecoValido()
            },
            Contrato = new ContratoFinanceiro
            {
                TipoContrato = TipoContrato.ContratoAluno,
                ValorMensal = 500,
                Vencimento = "10"
            }
        };
    }

    public static EnderecoPessoa CriarEnderecoValido()
    {
        return new EnderecoPessoa
        {
            Logradouro = "Rua A",
            Numero = "100",
            Bairro = "Centro",
            Cidade = "São Paulo",
            Estado = "SP",
            CEP = "01001000"
        };
    }

    public static MovimentoFinanceiroEntidade CriarMovimentoProfissional(string competencia = "07/2026")
    {
        return new MovimentoFinanceiroEntidade
        {
            ContratoId = Guid.NewGuid(),
            CompetenciaMensalidade = competencia,
            Classe = ClasseMovimento.Profissional
        };
    }

    public static ContratoFinanceiro CriarContratoProfissionalAtivo()
    {
        return new ContratoFinanceiro
        {
            Id = Guid.NewGuid(),
            TipoContrato = TipoContrato.ContratoProfissional,
            ValorUnitario = 100,
            Quantidade = 5,
            MargemLucro = 20,
            Vencimento = "10",
            Entidade = new Entidade
            {
                DataSaida = DateTime.MinValue
            }
        };
    }
}
