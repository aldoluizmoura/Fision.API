using FIsionAPI.API.Authentication.Jwt;
using FIsionAPI.API.Authentication.Models;
using FIsionAPI.API.Controllers;
using FIsionAPI.API.ViewModels;
using FIsionAPI.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FIsionAPI.API.V1.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(INotificador notificador,
                          UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          ITokenService tokenService) : base(notificador)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("registrar")]
    [AllowAnonymous]
    public async Task<ActionResult> Registrar(RegistrarUsuarioViewModel viewModel)
    {
        if (!ModelState.IsValid) return CustomReponse(ModelState);

        var user = new User
        {
            UserName = viewModel.Email,
            Email = viewModel.Email,
            Nome = viewModel.Nome,
            Documento = viewModel.Documento,
            EmailConfirmed = true,
            DataCadastro = DateTime.UtcNow,
            Ativo = true
        };

        var resultado = await _userManager.CreateAsync(user, viewModel.Senha);

        if (!resultado.Succeeded)
        {
            foreach (var erro in resultado.Errors)
                NotificarErro(erro.Description);

            return CustomResponse();
        }

        var token = await _tokenService.GerarTokenAsync(user);
        return CustomResponse(token);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginUsuarioViewModel viewModel)
    {
        if (!ModelState.IsValid) return CustomReponse(ModelState);

        var user = await _userManager.FindByEmailAsync(viewModel.Email);

        if (user is null)
        {
            NotificarErro("Usuário ou senha incorretos");
            return CustomResponse();
        }

        if (!user.Ativo)
        {
            NotificarErro("Usuário inativo. Contate o administrador.");
            return CustomResponse();
        }

        var resultado = await _signInManager.CheckPasswordSignInAsync(user, viewModel.Senha, lockoutOnFailure: true);

        if (resultado.IsLockedOut)
        {
            NotificarErro("Usuário temporariamente bloqueado por excesso de tentativas.");
            return CustomResponse();
        }

        if (!resultado.Succeeded)
        {
            NotificarErro("Usuário ou senha incorretos");
            return CustomResponse();
        }

        user.UltimoLogin = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await _tokenService.GerarTokenAsync(user);
        return CustomResponse(token);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var token = await _tokenService.ObterRefreshTokenAsync(refreshToken);
        if (token == null || token.User == null)
        {
            NotificarErro("Refresh token inválido ou expirado.");
            return CustomResponse();
        }
        var response = await _tokenService.GerarTokenAsync(token.User);
        return CustomResponse(response);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] string refreshToken)
    {
        await _tokenService.RevogarRefreshTokenAsync(refreshToken);
        return CustomResponse();
    }
}
