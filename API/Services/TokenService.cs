using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

// TokenService is responsible for generating JWT tokens for authenticated users
public class TokenService(IConfiguration config) : ITokenService
{
    
    // Creates a JWT token for the given user
    public string CreateToken(AppUser user)
    {
        // Read the secret key from configuration (appsettings.json or environment variables)
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot get token key");
        
        if (tokenKey.Length < 64) throw new Exception("Your token key need to be >= 64 characters");
        
        // Convert the secret key string into bytes and create a symmetric security key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        // Define claims that will be stored inside the JWT
        // These claims identify the user when the token is validated later
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Describe the contents and rules of the JWT
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };
        
        // JWT handler responsible for creating and writing tokens
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
