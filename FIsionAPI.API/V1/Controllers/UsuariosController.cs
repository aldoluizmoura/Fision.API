using FIsionAPI.API.Authentication;
using FIsionAPI.API.Authentication.Models;
using FIsionAPI.API.Controllers;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIsionAPI.API.V1.Controllers;

[Authorize(Policy = Policies.RequerAdmin)]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/usuarios")]
public class UsuariosController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsuariosController(INotificador notificador,
                              UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager) : base(notificador)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<ActionResult> Listar([FromQuery] string email = null, [FromQuery] string nome = null)
    {
        var users = _userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(email))
            users = users.Where(u => u.Email.Contains(email));
        if (!string.IsNullOrWhiteSpace(nome))
            users = users.Where(u => u.Nome.Contains(nome));

        var result = new List<UsuarioViewModel>();

        foreach (var user in users.ToList())
        {
            result.Add(await MapearUsuarioAsync(user));
        }

        return CustomResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> ObterPorId(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            NotificarErro("Usuário não encontrado");
            return CustomResponse();
        }

        return CustomResponse(await MapearUsuarioAsync(user));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(string id, AtualizarUsuarioViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            NotificarErro("Id inválido");
            return CustomResponse();
        }

        if (!ModelState.IsValid)
            return CustomReponse(ModelState);

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            NotificarErro("Usuário não encontrado");
            return CustomResponse();
        }

        user.Nome = viewModel.Nome;
        user.Documento = viewModel.Documento;

        var resultado = await _userManager.UpdateAsync(user);

        if (!resultado.Succeeded)
        {
            foreach (var erro in resultado.Errors)
                NotificarErro(erro.Description);

            return CustomResponse();
        }

        return CustomResponse(await MapearUsuarioAsync(user));
    }

    [HttpPut("{id}/ativar")]
    public async Task<ActionResult> Ativar(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            NotificarErro("Usuário não encontrado");
            return CustomResponse();
        }

        user.Ativo = true;
        await _userManager.UpdateAsync(user);

        return CustomResponse(await MapearUsuarioAsync(user));
    }

    [HttpPut("{id}/desativar")]
    public async Task<ActionResult> Desativar(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            NotificarErro("Usuário não encontrado");
            return CustomResponse();
        }

        user.Ativo = false;
        await _userManager.UpdateAsync(user);

        return CustomResponse(await MapearUsuarioAsync(user));
    }

    [HttpPut("resetar-senha")]
    public async Task<ActionResult> ResetarSenha(AlterarSenhaViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return CustomReponse(ModelState);

        var user = await _userManager.FindByIdAsync(viewModel.UserId);

        if (user == null)
        {
            NotificarErro("Usuário não encontrado");
            return CustomResponse();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resultado = await _userManager.ResetPasswordAsync(user, token, viewModel.NovaSenha);

        if (!resultado.Succeeded)
        {
            foreach (var erro in resultado.Errors)
                NotificarErro(erro.Description);

            return CustomResponse();
        }

        return CustomResponse();
    }

    [HttpPut("gerenciar-roles")]
    public async Task<ActionResult> GerenciarRoles(UsuarioRoleViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return CustomReponse(ModelState);

        var user = await _userManager.FindByIdAsync(viewModel.UserId);

        if (user == null)
        {
            NotificarErro("Usuário não encontrado");
            return CustomResponse();
        }

        foreach (var role in viewModel.Roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                NotificarErro($"Role '{role}' não existe");
                return CustomResponse();
            }
        }

        var rolesAtuais = await _userManager.GetRolesAsync(user);
        var remover = rolesAtuais.Except(viewModel.Roles).ToList();
        var adicionar = viewModel.Roles.Except(rolesAtuais).ToList();

        if (remover.Any())
        {
            var resultadoRemover = await _userManager.RemoveFromRolesAsync(user, remover);

            if (!resultadoRemover.Succeeded)
            {
                foreach (var erro in resultadoRemover.Errors)
                    NotificarErro(erro.Description);

                return CustomResponse();
            }
        }

        if (adicionar.Any())
        {
            var resultadoAdicionar = await _userManager.AddToRolesAsync(user, adicionar);

            if (!resultadoAdicionar.Succeeded)
            {
                foreach (var erro in resultadoAdicionar.Errors)
                    NotificarErro(erro.Description);

                return CustomResponse();
            }
        }

        return CustomResponse(await MapearUsuarioAsync(user));
    }

    private async Task<UsuarioViewModel> MapearUsuarioAsync(User user)
    {
        return new UsuarioViewModel
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email,
            Documento = user.Documento,
            Ativo = user.Ativo,
            DataCadastro = user.DataCadastro,
            UltimoLogin = user.UltimoLogin,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }
}
