﻿using AutoMapper;
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
    [Route("api/v{version:apiVersion}/entidades")]
    public class EntidadeController : BaseController
    {
        private readonly IEntidadeRepository _entidadeRepository;
        private readonly IEntidadeService _entidadeService;
        private readonly IMapper _mapper;
        public EntidadeController(INotificador notificador, IMapper mapper,
                                  IEntidadeService entidadeService,
                                  IEntidadeRepository entidadeRepository) : base(notificador)
        {
            _entidadeRepository = entidadeRepository;
            _entidadeService = entidadeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<EntidadeViewModel>> MostrarTodos()
        {
            var entidade = _mapper.Map<IEnumerable<EntidadeViewModel>>
                  (await _entidadeRepository.ObterTodos());
            return entidade;
        }
        [HttpGet("{id:guid}")]
        public async Task<EntidadeViewModel> MostrarPorId(Guid id)
        {
            var entidade = _mapper.Map<EntidadeViewModel>
                (await _entidadeRepository.ObterPorEntidadeId(id));

            return entidade;
        }

        [HttpPost]
        public async Task<ActionResult<EntidadeViewModel>> Adicionar(EntidadeViewModel entidadeViewModel)
        {
            if (!ModelState.IsValid) return CustomReponse(ModelState);

            var entidade = _mapper.Map<Entidade>(entidadeViewModel);
            await _entidadeService.Adicionar(entidade);

            return CustomResponse(entidadeViewModel);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EntidadeViewModel>> Atualizar(Guid id, EntidadeViewModel entidadeViewlModel)
        {
            if (id != entidadeViewlModel.Id) return BadRequest();
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var entidade = _mapper.Map<Entidade>(entidadeViewlModel);
            await _entidadeService.Atualizar(entidade);

            return CustomResponse(entidadeViewlModel);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<EntidadeViewModel>> Remover(Guid id)
        {
            var entidadeViewModel = await _entidadeRepository.ObterPorEntidadeId(id);
            if (entidadeViewModel == null) return NotFound();

            await _entidadeService.Remover(id);

            return CustomResponse();
        }
        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<ActionResult> AtualizarEndereco(Guid id, EnderecoPessoaViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(enderecoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _entidadeService.AtualizarEndereco(_mapper.Map<EnderecoPessoa>(enderecoViewModel));

            return CustomResponse(enderecoViewModel);
        }
        [HttpPut("atualizar-contrato/{id:guid}")]
        public async Task<ActionResult> AtualizarContrato(Guid id, ContratoFinanceiroViewModel contratoViewModel)
        {
            if (id != contratoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(contratoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _entidadeService.AtualizarContratoFinanceiro(_mapper.Map<ContratoFinanceiro>(contratoViewModel));

            return CustomResponse(contratoViewModel);
        }
        [HttpPut("atualizar-pessoa/{id:guid}")]
        public async Task<ActionResult> AtualizarPessoa(Guid id, PessoaViewModel pessoaViewModel)
        {
            if (id != pessoaViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(pessoaViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var pessoa = _mapper.Map<Pessoa>(pessoaViewModel);
            await _entidadeService.AtualizarPessoa(pessoa);

            return CustomResponse(pessoaViewModel);
        }
    }
}
