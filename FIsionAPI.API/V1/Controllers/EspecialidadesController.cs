using AutoMapper;
using FIsionAPI.API.Authentication;
using FIsionAPI.API.Controllers;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIsionAPI.API.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/especialidades")]
public class EspecialidadesController : BaseController
{
    private readonly IEspecialidadeRepository _especialidadeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EspecialidadesController(INotificador notificador,
                                    IEspecialidadeRepository especialidadeRepository,
                                    IUnitOfWork unitOfWork,
                                    IMapper mapper) : base(notificador)
    {
        _especialidadeRepository = especialidadeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult> Listar()
    {
        var especialidades = await _especialidadeRepository.ObterEspecialidades();
        return CustomResponse(_mapper.Map<IEnumerable<EspecialidadeViewModel>>(especialidades));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> ObterPorId(Guid id)
    {
        var especialidade = await _especialidadeRepository.ObterEspecialidadePorId(id);

        if (especialidade == null)
        {
            NotificarErro("Especialidade não encontrada");
            return CustomResponse();
        }

        return CustomResponse(_mapper.Map<EspecialidadeViewModel>(especialidade));
    }

    [HttpPost]
    [Authorize(Policy = Policies.RequerGestor)]
    public async Task<ActionResult> Adicionar(EspecialidadeViewModel viewModel)
    {
        if (string.IsNullOrWhiteSpace(viewModel.Descricao))
        {
            NotificarErro("A descrição é obrigatória");
            return CustomResponse();
        }

        var especialidade = _mapper.Map<Especialidades>(viewModel);
        await _especialidadeRepository.Adicionar(especialidade);
        await _unitOfWork.Commit();

        return CustomResponse(_mapper.Map<EspecialidadeViewModel>(especialidade));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policies.RequerGestor)]
    public async Task<ActionResult> Atualizar(Guid id, EspecialidadeViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            NotificarErro("Id inválido");
            return CustomResponse();
        }

        if (string.IsNullOrWhiteSpace(viewModel.Descricao))
        {
            NotificarErro("A descrição é obrigatória");
            return CustomResponse();
        }

        var especialidade = await _especialidadeRepository.ObterEspecialidadePorId(id);

        if (especialidade == null)
        {
            NotificarErro("Especialidade não encontrada");
            return CustomResponse();
        }

        especialidade.Descricao = viewModel.Descricao;
        await _especialidadeRepository.Atualizar(especialidade);
        await _unitOfWork.Commit();

        return CustomResponse(_mapper.Map<EspecialidadeViewModel>(especialidade));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.RequerAdmin)]
    public async Task<ActionResult> Remover(Guid id)
    {
        var especialidade = await _especialidadeRepository.ObterEspecialidadePorId(id);

        if (especialidade == null)
        {
            NotificarErro("Especialidade não encontrada");
            return CustomResponse();
        }

        await _especialidadeRepository.Remover(id);
        await _unitOfWork.Commit();

        return CustomResponse();
    }
}
