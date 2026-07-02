using FIsionAPI.Business.Models.Validacões.Documentos;

namespace FIsionAPI.Tests.Validacoes;

public class ValidacaoDocsTests
{
    [Theory]
    [InlineData("11144477735", true)]
    [InlineData("00000000000", false)]
    [InlineData("123", false)]
    [InlineData("", false)]
    public void ValidarCpf_DeveRetornarResultadoEsperado(string cpf, bool esperado)
    {
        var resultado = ValidacaoDocs.Validar(cpf);

        Assert.Equal(esperado, resultado);
    }
}
