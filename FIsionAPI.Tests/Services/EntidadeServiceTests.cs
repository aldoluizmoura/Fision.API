using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Notificacões;
using FIsionAPI.Business.Services;
using FIsionAPI.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FIsionAPI.Tests.Services;

public class EntidadeServiceTests
{
    private readonly Mock<IPessoaRepositoy> _pessoaRepository = new();
    private readonly Mock<IEntidadeRepository> _entidadeRepository = new();
    private readonly Mock<IContratoFinanceiroRepository> _contratoRepository = new();
    private readonly Mock<IEnderecoPessoaRepository> _enderecoRepository = new();
    private readonly Mock<IMovimentoFinanceiroRepository> _movimentoRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Notificador _notificador = new();

    private EntidadeService CriarServico()
    {
        return new EntidadeService(
            _notificador,
            _pessoaRepository.Object,
            _enderecoRepository.Object,
            _contratoRepository.Object,
            _movimentoRepository.Object,
            _entidadeRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task Adicionar_DevePersistir_QuandoDadosForemValidos()
    {
        var entidade = TestDataHelper.CriarEntidadeAlunoValida();

        _pessoaRepository
            .Setup(r => r.Buscar(It.IsAny<Expression<Func<Pessoa, bool>>>()))
            .ReturnsAsync(new List<Pessoa>());

        _entidadeRepository
            .Setup(r => r.Adicionar(It.IsAny<Entidade>()))
            .Returns(Task.CompletedTask);

        _unitOfWork
            .Setup(u => u.Commit())
            .ReturnsAsync(true);

        var servico = CriarServico();
        var resultado = await servico.Adicionar(entidade);

        Assert.True(resultado);
        _entidadeRepository.Verify(r => r.Adicionar(entidade), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task Adicionar_DeveFalhar_QuandoCpfJaExistir()
    {
        var entidade = TestDataHelper.CriarEntidadeAlunoValida();

        _pessoaRepository
            .Setup(r => r.Buscar(It.IsAny<Expression<Func<Pessoa, bool>>>()))
            .ReturnsAsync(new List<Pessoa> { new() { CPF = TestDataHelper.CpfValido } });

        var servico = CriarServico();
        var resultado = await servico.Adicionar(entidade);

        Assert.False(resultado);
        Assert.True(_notificador.TemNotificacao());
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Adicionar_DeveFalhar_QuandoEnderecoForInvalido()
    {
        var entidade = TestDataHelper.CriarEntidadeAlunoValida();
        entidade.Pessoa.Endereco.CEP = string.Empty;

        var servico = CriarServico();
        var resultado = await servico.Adicionar(entidade);

        Assert.False(resultado);
        Assert.True(_notificador.TemNotificacao());
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }
}
