using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIsionAPI.API.ViewModels;

public class UsuarioViewModel
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Documento { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime? UltimoLogin { get; set; }
    public List<string> Roles { get; set; }
}

public class UsuarioRoleViewModel
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public List<string> Roles { get; set; }
}

public class AlterarSenhaViewModel
{
    [Required]
    public string UserId { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string NovaSenha { get; set; }
}

public class AtualizarUsuarioViewModel
{
    [Required]
    public string Id { get; set; }
    [Required]
    [StringLength(150)]
    public string Nome { get; set; }
    [StringLength(20)]
    public string Documento { get; set; }
}
