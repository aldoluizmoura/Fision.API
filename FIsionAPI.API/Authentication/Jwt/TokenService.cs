using FIsionAPI.API.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FIsionAPI.API.Authentication.Jwt;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly Authentication.AuthenticationConfig.AuthenticationDbContext _db;

    public TokenService(IOptions<JwtSettings> jwtSettings, UserManager<User> userManager, Authentication.AuthenticationConfig.AuthenticationDbContext db)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _db = db;
    }

    public async Task<TokenResponse> GerarTokenAsync(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var claims = await MontarClaimsAsync(user);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiraEm = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiraEm,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        // Gera e persiste refresh token
        var refreshToken = await GerarRefreshTokenAsync(user);

        return new TokenResponse
        {
            AccessToken = accessToken,
            ExpiresIn = _jwtSettings.ExpirationMinutes * 60,
            ExpiresAt = expiraEm,
            TokenType = "Bearer",
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
    }

    public async Task<RefreshToken> GerarRefreshTokenAsync(User user)
    {
        // Revoga tokens antigos do usuário
        var antigos = await _db.RefreshTokens.Where(r => r.UserId == user.Id && r.RevokedAt == null && !r.IsExpired).ToListAsync();
        foreach (var t in antigos)
        {
            t.RevokedAt = DateTime.UtcNow;
        }

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Guid.NewGuid().ToString("N");
        var expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expires
        };
        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken> ObterRefreshTokenAsync(string token)
    {
        var refreshToken = await _db.RefreshTokens.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
        if (refreshToken == null || !refreshToken.IsActive)
            return null;
        return refreshToken;
    }

    public async Task RevogarRefreshTokenAsync(string token)
    {
        var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
        if (refreshToken != null && refreshToken.RevokedAt == null)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }

    private async Task<IEnumerable<Claim>> MontarClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new("nome", user.Nome ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        if (user.PessoaId.HasValue)
            claims.Add(new Claim("pessoaId", user.PessoaId.Value.ToString()));

        if (!string.IsNullOrWhiteSpace(user.Documento))
            claims.Add(new Claim("documento", user.Documento));

        // Roles
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        // Claims customizadas persistidas
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        return claims;
    }
}
