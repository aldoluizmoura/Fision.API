using Microsoft.AspNetCore.Identity;

namespace FIsionAPI.API.Authentication.Models;

public class User : IdentityUser
{
    public string Documento { get; set; }
}
