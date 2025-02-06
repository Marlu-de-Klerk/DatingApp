using System;
using API.Interfaces;
using API.Entities;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user){

        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access TokenKey from AppSettings");
        if (tokenKey.Length < 64 ) throw new Exception("Your Tokenkey needs to be longer");
        var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserName)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds

        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
