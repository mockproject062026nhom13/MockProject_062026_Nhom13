using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NursingHome.Application.Abstractions.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NursingHome.Infrastructure.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    //create access token
    private readonly IConfiguration _configuration = configuration;


    public string GenerateAccessToken(long userId,string email)
    {
        //read configuration from appsettings.jason
        var secretKey = _configuration["JwtSettings:SecretKey"]?? throw new InvalidOperationException("CRITICAL: Thiếu JwtSettings:SecretKey trong appsettings.json");
        var issuer = _configuration["JwtSettings: Issuer"];
        var audience = _configuration["JwtSettigs: Audience"];
        var expiryMinutes = Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"]);

        //convert the Secret Key into a byte array for encryption
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

        //Create payload
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

        };

        //assemble the components into a complete JWT token
        var token = new JwtSecurityToken(
            issuer:issuer, 
            audience: audience, 
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials:credentials);
        
        //Export Token
        return new JwtSecurityTokenHandler().WriteToken(token);

    }    

        

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}