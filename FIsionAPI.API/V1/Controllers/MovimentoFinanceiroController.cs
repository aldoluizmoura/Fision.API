using AutoMapper;
using FIsionAPI.API.Controllers;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.API.V1.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/movimento-financeiro")]
public class MovimentoFinanceiroController : BaseController
{
    private readonly IMovimentoFinanceiroRepository _movimentoRepository;
    private readonly IMovimentoFinanceiroService _movimentoService;
    private readonly IMovimentoFinanceiroAvulsoService _movimentoAvulsoService;
    private readonly IContratoFinanceiroRepository _contratoRepository;
    private readonly IMapper _mapper;

    public MovimentoFinanceiroController(INotificador notification,
                                         IMapper mapper,
                                         IContratoFinanceiroRepository contratoRepository,
                                         IMovimentoFinanceiroService movimentoService,
                                         IMovimentoFinanceiroAvulsoService movimentoAvulsoService,
                                         IMovimentoFinanceiroRepository movimentoRepository) : base(notification)
    {
        _movimentoRepository = movimentoRepository;
        _contratoRepository = contratoRepository;
        _movimentoService = movimentoService;
        _movimentoAvulsoService = movimentoAvulsoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<MovimentoFinanceiroViewModel>> MostrarTodos()
    {
        var movimentosFinanceiros = await _movimentoRepository.ObterTodos();
        var movimentosFinanceirosViewModel = _mapper.Map<IEnumerable<MovimentoFinanceiroViewModel>>(movimentosFinanceiros);

        return movimentosFinanceirosViewModel;
    }

    [HttpGet("id:guid")]
    public async Task<MovimentoFinanceiroViewModel> MostrarPorId(Guid Id)
    {
        var movimentosFinanceiros = await _movimentoRepository.ObterPorId(Id);
        var movimentosFinanceirosViewModel = _mapper.Map<MovimentoFinanceiroViewModel>(movimentosFinanceiros);

        return movimentosFinanceirosViewModel;
    }

    [HttpPost("adicionarMensalidade-movimentoEntidade")]
    public async Task<ActionResult<MovimentoFinanceiroViewModel>> AdicionarMensalidade(MovimentoFinanceiroViewModel movimentoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponse(ModelState);
        }

        var movimento = _mapper.Map<MovimentoFinanceiroEntidade>(movimentoViewModel);

        await _movimentoService.AdicionarMensalidade(movimento);

        return CustomResponse(movimentoViewModel);
    }

    [HttpPost("adicionarValorProfissional-movimentoEntidade")]
    public async Task<ActionResult<MovimentoFinanceiroViewModel>> AdicionarValorProfissional(MovimentoFinanceiroViewModel movimentoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponse(ModelState);
        }

        var movimento = _mapper.Map<MovimentoFinanceiroEntidade>(movimentoViewModel);

        await _movimentoService.AdicionarValorProfissional(movimento);

        return CustomResponse(movimentoViewModel);
    }

    [HttpPost("adicionar-movimentoAvulso")]
    public async Task<ActionResult<MovimentoFinanceiroViewModel>> AdicionarMovimentoAvulso(MovimentoFinanceiroAvulsoViewModel movimentoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return CustomReponse(ModelState);
        }

        var movimento = _mapper.Map<MovimentoFinanceiroAvulso>(movimentoViewModel);

        await _movimentoAvulsoService.AdicionarMovimentoFinanceiroAvulso(movimento);

        return CustomResponse(movimentoViewModel);
    }

    [HttpPut("quitar-movimento/id:guid")]
    public async Task<ActionResult> Quitar(Guid id)
    {
        var movimentoViewModel = await ObterMovimento(id);

        if (movimentoViewModel == null)
        {
            NotificarErro("Movimento não encontrado!");
            return CustomResponse(_mapper.Map<MovimentoFinanceiroViewModel>(movimentoViewModel));
        }

        await _movimentoService.QuitarMensalidade(movimentoViewModel);

        return CustomResponse(movimentoViewModel);
    }

    [HttpPut("desquitar-movimento/id:guid")]
    public async Task<ActionResult> Desquitar(Guid id)
    {
        var movimentoViewModel = await ObterMovimento(id);

        if (movimentoViewModel == null)
        {
            NotificarErro("Movimento não encontrado!");
            return CustomResponse(_mapper.Map<MovimentoFinanceiroViewModel>(movimentoViewModel));
        }

        await _movimentoService.Desquitar(movimentoViewModel.Id);

        return CustomResponse(movimentoViewModel);
    }

    [HttpDelete]
    public async Task<ActionResult> Excluir(Guid id)
    {
        var movimentoViewModel = await ObterMovimento(id);

        if (movimentoViewModel == null)
        {
            NotificarErro("Movimento não encontrado!");
            return CustomResponse(_mapper.Map<MovimentoFinanceiroViewModel>(movimentoViewModel));
        }

        await _movimentoService.Remover(movimentoViewModel.Id);

        return CustomResponse(movimentoViewModel);
    }

    private async Task<MovimentoFinanceiroEntidade> ObterMovimento(Guid id)
    {
        var movimento = await _movimentoRepository.ObterPorId(id);
        return movimento;
    }
}
