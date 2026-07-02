namespace FIsionAPI.API.Authentication;

/// <summary>
/// Nomes das policies de autorização utilizadas no projeto.
/// </summary>
public static class Policies
{
    public const string RequerAdmin = nameof(RequerAdmin);
    public const string RequerGestor = nameof(RequerGestor);
    public const string RequerUsuario = nameof(RequerUsuario);
}
