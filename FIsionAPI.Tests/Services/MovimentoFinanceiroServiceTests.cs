using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using FIsionAPI.Business.Notificacões;
using FIsionAPI.Business.Services;
using FIsionAPI.Tests.Helpers;
using Moq;
using System.Threading.Tasks;

namespace FIsionAPI.Tests.Services;

public class MovimentoFinanceiroServiceTests
{
    private readonly Mock<IMovimentoFinanceiroRepository> _movimentoRepository = new();
    private readonly Mock<IContratoFinanceiroRepository> _contratoRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Notificador _notificador = new();

    private MovimentoFinanceiroService CriarServico()
    {
        return new MovimentoFinanceiroService(
            _notificador,
            _contratoRepository.Object,
            _movimentoRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task AdicionarValorProfissional_DevePersistirCompetenciaMensalidade()
    {
        var movimento = TestDataHelper.CriarMovimentoProfissional("07/2026");
        var contrato = TestDataHelper.CriarContratoProfissionalAtivo();
        movimento.ContratoId = contrato.Id;

        MovimentoFinanceiroEntidade? movimentoPersistido = null;

        _contratoRepository
            .Setup(r => r.ObterContratoPorId(contrato.Id))
            .ReturnsAsync(contrato);

        _movimentoRepository
            .Setup(r => r.BuscarMovimentosPorCompetencia(movimento))
            .ReturnsAsync((MovimentoFinanceiroEntidade?)null);

        _movimentoRepository
            .Setup(r => r.Adicionar(It.IsAny<MovimentoFinanceiroEntidade>()))
            .Callback<MovimentoFinanceiroEntidade>(m => movimentoPersistido = m)
            .Returns(Task.CompletedTask);

        _unitOfWork
            .Setup(u => u.Commit())
            .ReturnsAsync(true);

        var servico = CriarServico();
        var resultado = await servico.AdicionarValorProfissional(movimento);

        Assert.True(resultado);
        Assert.NotNull(movimentoPersistido);
        Assert.Equal("07/2026", movimentoPersistido.CompetenciaMensalidade);
        _unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task AdicionarValorProfissional_DeveFalhar_QuandoEntidadeEstiverInativa()
    {
        var movimento = TestDataHelper.CriarMovimentoProfissional();
        var contrato = TestDataHelper.CriarContratoProfissionalAtivo();
        contrato.Entidade.DataSaida = System.DateTime.UtcNow;
        movimento.ContratoId = contrato.Id;

        _contratoRepository
            .Setup(r => r.ObterContratoPorId(contrato.Id))
            .ReturnsAsync(contrato);

        var servico = CriarServico();
        var resultado = await servico.AdicionarValorProfissional(movimento);

        Assert.False(resultado);
        Assert.True(_notificador.TemNotificacao());
        _movimentoRepository.Verify(r => r.Adicionar(It.IsAny<MovimentoFinanceiroEntidade>()), Times.Never);
    }
}
