using Microsoft.AspNetCore.Identity;
using System;

namespace FIsionAPI.API.Authentication.Models;

public class User : IdentityUser
{
    public string Documento { get; set; }

    public string Nome { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public DateTime? UltimoLogin { get; set; }

    public bool Ativo { get; set; } = true;

    /// <summary>
    /// Vínculo lógico com a entidade Pessoa do domínio (contexto separado).
    /// </summary>
    public Guid? PessoaId { get; set; }
}
