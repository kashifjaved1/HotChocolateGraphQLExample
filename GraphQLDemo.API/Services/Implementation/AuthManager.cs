﻿using GraphQLDemo.API.GraphQL.Types;
using GraphQLDemo.API.Helpers;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Entities;
using GraphQLDemo.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Services.Implementation
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private SigningCredentials GetSigningCredentials()
        {
            // To get key from environment variable use following line of code.
            // Environment.GetEnvironmentVariable("key_env_variable_name");
            var key = _configuration["JWT:KEY"];
            var keyInBytes = Encoding.UTF8.GetBytes(key);
            var symmeticSecurityKey = new SymmetricSecurityKey(keyInBytes);
            return new SigningCredentials(symmeticSecurityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, _user.Id),
                new Claim(ClaimTypes.Name, _user.FullName),
                new Claim(ClaimTypes.Email, _user.Email),
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var permissions = GetUserValidPermissions(new() { "create", "view" });
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permissions", permission));
            }

            return claims;
        }

        private JwtSecurityToken GenerateToken(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expiry = DateTime.Now.AddHours(1);
            var issuer = _configuration["JWT:Issuer"];
            var audiance = _configuration["JWT:Audiance"];
            var token = new JwtSecurityToken(
                issuer, audiance, claims, expires: expiry, signingCredentials: signingCredentials
            );

            return token;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateToken(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ValidateUser(LoginType login)
        {
            _user = await _userManager.FindByEmailAsync(login.Email);
            return _user != null && await _userManager.CheckPasswordAsync(_user, login.Password);
        }

        private List<string> GetUserValidPermissions(List<string> permissions)
        {
            permissions = Commons.CapsFirstLetter(permissions);

            var _permissions = Permissions.GetAllPermissions();

            var userPermissions = permissions
            .Where(p => _permissions.Contains($"Permissions.{p}", StringComparer.OrdinalIgnoreCase))
            .ToList();

            var userLongPermissions = _permissions
            .Where(p => permissions.Contains(p.Replace("Permissions.", ""), StringComparer.OrdinalIgnoreCase))
            .ToList();

            return userPermissions;
        }
    }
}