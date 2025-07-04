﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Lookif.Library.Common;
using Lookif.Layers.Core.Infrastructure.Base; 
using Lookif.Layers.Core.Else.JWT;
using Microsoft.AspNetCore.Identity; 

namespace Lookif.Layers.Service.Jwt;


public class JwtService<TUser> : IJwtService<TUser>, IScopedDependency
where TUser : IdentityUser<Guid>
{
    private readonly SiteSettings _siteSetting;
    private readonly SignInManager<TUser> signInManager; 


    public JwtService(
        IOptionsSnapshot<SiteSettings> settings,
        SignInManager<TUser> signInManager )
    {
        _siteSetting = settings.Value;
        this.signInManager = signInManager; 
    }

    public async Task<AccessToken> GenerateAsync(TUser user)
    {
        var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey);  
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

      
        var claims = await _getClaimsAsync(user);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _siteSetting.JwtSettings.Issuer,
            Audience = _siteSetting.JwtSettings.Audience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
            Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            //ToDo Make it Secure 
            Subject = new ClaimsIdentity(claims)
        }; 

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor); 

        return new AccessToken(securityToken);
    }

    private async Task<IEnumerable<Claim>> _getClaimsAsync(TUser user)
    {
        var result = await signInManager.ClaimsFactory.CreateAsync(user);
        var claims = new List<Claim>(result.Claims);


        return claims;
    }
}
