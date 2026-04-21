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

        var result = users.Select(u => new UsuarioViewModel
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Documento = u.Documento,
            Ativo = u.Ativo,
            DataCadastro = u.DataCadastro,
            UltimoLogin = u.UltimoLogin,
            Roles = new List<string>()
        }).ToList();

        foreach (var user in result)
        {
            var userObj = await _userManager.FindByIdAsync(user.Id);
            user.Roles = (await _userManager.GetRolesAsync(userObj)).ToList();
        }

        return CustomResponse(result);
    }

    // Endpoints de detalhe, ativar/desativar, resetar senha, atualizar dados e gerenciar roles serão implementados na sequência.
}
