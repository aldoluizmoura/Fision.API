using System.ComponentModel.DataAnnotations;

namespace FIsionAPI.API.ViewModels;

public class RegistrarUsuarioViewModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(150, ErrorMessage = "O campo Nome deve ter no máximo {1} caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email em formato inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "O campo Senha deve ter entre {2} e {1} caracteres")]
    public string Senha { get; set; }

    [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem")]
    public string ConfirmacaoSenha { get; set; }

    [StringLength(20, ErrorMessage = "O campo Documento deve ter no máximo {1} caracteres")]
    public string Documento { get; set; }
}

public class LoginUsuarioViewModel
{
    [Required(ErrorMessage = "O campo Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email em formato inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório")]
    public string Senha { get; set; }
}

public class TokenRespostaViewModel
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public System.DateTime ExpiresAt { get; set; }
    public string RefreshToken { get; set; }
    public System.DateTime RefreshTokenExpiresAt { get; set; }
}

public class RefreshTokenRequestViewModel
{
    [Required]
    public string RefreshToken { get; set; }
}
