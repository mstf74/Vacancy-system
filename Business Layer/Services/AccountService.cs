using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Dtos;
using Business_Layer.ServicesInterfaces;
using Data_Access_Layer.Enums;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business_Layer.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configurationManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configurationManager
        )
        {
            _userManager = userManager;
            _configurationManager = configurationManager;
        }

        public async Task<IdentityResult> Register(RegisterDto userDto)
        {
            // email, username, phone number and Rule is validated by the RegisterDtoValidator.
            var userModel = MapDtoToModel(userDto);
            var RegisterationResult = await _userManager.CreateAsync(userModel, userDto.password);
            return RegisterationResult;
        }

        public async Task<IdentityResult> Login(LoginDto userDto)
        {
            // email and not existed user is validated by the LoginDtoVAlidator.
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var result = await _userManager.CheckPasswordAsync(user, userDto.Password);
            if (result)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(
                    errors: new IdentityError()
                    {
                        Code = "invalidPassword",
                        Description = "Invalid password",
                    }
                );
        }

        public async Task<string> GeneratToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;
            var secret = _configurationManager["Jwt:Key"];
            var minutes = _configurationManager["Jwt:ExpiryDate"];
            int expiryDate;
            if (!int.TryParse(minutes, out expiryDate))
            {
                expiryDate = 30;
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, user.Rule.ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _configurationManager["Jwt:Issuer"],
                audience: _configurationManager["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryDate),
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ApplicationUser MapDtoToModel(RegisterDto userDto)
        {
            var userModel = new ApplicationUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PhoneNumber = userDto.phonenumber,
                Rule = userDto.Rule,
            };
            return userModel;
        }
    }
}
