namespace FIsionAPI.API.Authentication;

/// <summary>
/// Nomes padronizados das roles usadas na aplicação.
/// Centralizar aqui evita typos em atributos [Authorize(Roles = ...)].
/// </summary>
public static class Roles
{
    public const string Admin = "Admin";
    public const string Gestor = "Gestor";
    public const string Usuario = "Usuario";

    public static readonly string[] Todas = { Admin, Gestor, Usuario };
}
