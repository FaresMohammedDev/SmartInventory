using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartInventory.BL.Common;
using SmartInventory.BL.DTOs.Account;
using SmartInventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IConfiguration _configuration;
        public AccountService(UserManager<ApplicationUser> usermanager, IConfiguration configuration)
        {
            _usermanager = usermanager;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<AuthResponseDto>> Login(LoginDto login)
        {
            var user = await _usermanager.FindByEmailAsync(login.Email);
            if (user == null)
                return ServiceResponse<AuthResponseDto>.Fail("Email or Password invalid");

            var IsPasswordCorrect = await _usermanager.CheckPasswordAsync(user, login.Password);
            if (!IsPasswordCorrect)
                return ServiceResponse<AuthResponseDto>.Fail("Email or Password invalid");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, login.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(1),
                claims: claims,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return ServiceResponse<AuthResponseDto>.Success(
                new AuthResponseDto { token = tokenString, Expiration = token.ValidTo });
        }

        public async Task<ServiceResponse<string>> Register(RegisterDto register)
        {
            var existingEmail = await _usermanager.FindByEmailAsync(register.Email);
            if (existingEmail != null)
                return ServiceResponse<string>.Fail("Email already exists");

            var existingUser = await _usermanager.FindByNameAsync(register.UserName);
            if (existingUser != null)
                return ServiceResponse<string>.Fail("Username is already taken");

            if (register.Password != register.ConfirmPassword)
                return ServiceResponse<string>.Fail("Password must equal confirm password");

            var user = new ApplicationUser
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Email = register.Email,
            };

            var result = await _usermanager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(x => x.Description));
                return ServiceResponse<string>.Fail(error);
            }


            return ServiceResponse<string>.Success($"{user.Id}");
        }
    }
}
