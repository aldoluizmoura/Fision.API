using FIsionAPI.Business.Interfaces;
using FIsionAPI.Business.Notificacões;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Security.Claims;

namespace FIsionAPI.API.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    private readonly INotificador _notificador;

    public BaseController(INotificador notificador)
    {
        _notificador = notificador;
    }

    /// <summary>Indica se o usuário da requisição atual está autenticado.</summary>
    protected bool UsuarioAutenticado => User?.Identity?.IsAuthenticated ?? false;

    /// <summary>Id do usuário autenticado (claim 'sub' / NameIdentifier).</summary>
    protected string UsuarioId =>
        User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? User?.FindFirst("sub")?.Value;

    /// <summary>Email do usuário autenticado.</summary>
    protected string UsuarioEmail => User?.FindFirst(ClaimTypes.Email)?.Value;

    /// <summary>Nome amigável do usuário autenticado (claim customizada 'nome').</summary>
    protected string UsuarioNome => User?.FindFirst("nome")?.Value;

    /// <summary>PessoaId vinculada ao usuário autenticado, se houver.</summary>
    protected Guid? UsuarioPessoaId
    {
        get
        {
            var valor = User?.FindFirst("pessoaId")?.Value;
            return Guid.TryParse(valor, out var id) ? id : null;
        }
    }

    /// <summary>Verifica se o usuário autenticado pertence à role informada.</summary>
    protected bool UsuarioTemRole(string role) => User?.IsInRole(role) ?? false;

    protected bool OperacaoValida()
    {
        return !_notificador.TemNotificacao();
    }

    protected ActionResult CustomResponse(object result = null)
    {
        if (OperacaoValida())
        {
            return Ok(new
            {
                success = true,
                data = result
            });
        }

        return BadRequest(new
        {
            success = false,
            erros = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
        });
    }

    protected ActionResult CustomReponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
        {
            NotificarErroModelInvalida(modelState);
        }

        return CustomResponse();
    }

    protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);

        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            NotificarErro(errorMsg);
        }
    }

    protected void NotificarErro(string mensagem)
    {
        _notificador.Handle(new Notificacao(mensagem));
    }
}
