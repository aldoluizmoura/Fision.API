using FIsionAPI.Business.Models;
using FIsionAPI.Business.Models.Validacões;
using FIsionAPI.Tests.Helpers;

namespace FIsionAPI.Tests.Validacoes;

public class PessoaValidationTests
{
    private readonly PessoaValidation _validator = new();

    [Fact]
    public void DeveSerValida_QuandoPessoaPossuiDadosCompletos()
    {
        var entidade = TestDataHelper.CriarEntidadeAlunoValida();

        var resultado = _validator.Validate(entidade.Pessoa);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void DeveFalhar_QuandoCpfForInvalido()
    {
        var entidade = TestDataHelper.CriarEntidadeAlunoValida();
        entidade.Pessoa.CPF = "00000000000";

        var resultado = _validator.Validate(entidade.Pessoa);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(Pessoa.CPF));
    }
}
