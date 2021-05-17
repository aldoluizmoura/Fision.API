using AutoMapper;
using FIsionAPI.API.Controllers;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/caixa")]
    public class CaixaController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ICaixaRepository _caixaRepository;
        private readonly ICaixaService _caixaService;
        public CaixaController(INotificador notificador, ICaixaService caixaService,
                                ICaixaRepository caixaRepository,
                                IMapper mapper) 
                                : base (notificador)
        {
            _mapper = mapper;
            _caixaRepository = caixaRepository;
            _caixaService = caixaService;
        } 
        [HttpGet]
        public async Task<IEnumerable<CaixaViewModel>> MostrarTodos()
        {
            var caixas = _mapper.Map<IEnumerable<CaixaViewModel>>(await  _caixaRepository.ObterCaixas());
            return caixas;
        }
        [HttpGet("id:guid")]
        public async Task<CaixaViewModel> MostrarPorId(Guid id)
        {
            var caixas = _mapper.Map<CaixaViewModel>(await _caixaRepository.ObterPorId(id));
            return caixas;
        }
        [HttpPost("adicionar-caixa")]
        public async Task<ActionResult<CaixaViewModel>> Adicionar(CaixaViewModel caixaViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var caixa = _mapper.Map<Caixa>(caixaViewModel);

            await _caixaService.Adicionar(caixa);

            return CustomResponse(caixa);
        }
        [HttpDelete("excluir-caixa/id:guid")]
        public async Task<ActionResult> Excluir(Guid id)
        {
            var caixa = await ObterCaixa(id);
            if (caixa == null) 
            {
                NotificarErro("Caixa não encontrado");
                return CustomResponse(caixa);
            }
            await _caixaService.Remover(caixa);
            return CustomResponse(caixa);
        }
        [HttpPut("fechar-caixa/id:guid")]
        public async Task<ActionResult> FecharCaixa(Guid id)
        {
            var caixa = await ObterCaixa(id);
            if (caixa == null)
            {
                NotificarErro("Caixa não encontrado");
                return CustomResponse(caixa);
            }
            await _caixaService.FecharCaixa(caixa);
            return CustomResponse(caixa);
        }
        [HttpPut("reabrir-caixa/id:guid")]
        public async Task<ActionResult> ReabrirCaixa(Guid id)
        {
            var caixa = await ObterCaixa(id);
            if (caixa == null)
            {
                NotificarErro("Caixa não encontrado");
                return CustomResponse(caixa);
            }
            await _caixaService.ReabrirCaixa(caixa);
            return CustomResponse(caixa);
        }
        private async Task<Caixa> ObterCaixa(Guid id)
        {
            return _mapper.Map<Caixa>(await _caixaRepository.ObterPorId(id));
        }
    }
}
