using Business_Layer.Dtos;
using Business_Layer.Services;
using Business_Layer.ServicesInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Vacancy_system.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<RegisterDto> _registrationValidator;
        private readonly IValidator<LoginDto> _LoginValidator;

        public AccountController(IAccountService accountService, IValidator<RegisterDto> registrationValidator, IValidator<LoginDto> loginValidator)
        {
            _accountService = accountService;
            _registrationValidator = registrationValidator;
            _LoginValidator = loginValidator;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto Account)
        {
            var validationResult = await _registrationValidator.ValidateAsync(Account);
            if (!validationResult.IsValid)
            {
                var errorDetails = new HttpValidationProblemDetails(validationResult.ToDictionary());
                return BadRequest(errorDetails);
            }
            var result = await _accountService.Register(Account);
            if (!result.Succeeded)
            {
                var errorDetails = new HttpValidationProblemDetails(
                result.Errors.ToDictionary(
                  e => e.Code,
                  e => new string[] { e.Description }));
                return BadRequest(errorDetails);
            }
            var token = await _accountService.GeneratToken(Account.Email);
            return Ok(token);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto Account)
        {
            var validationResult = await _LoginValidator.ValidateAsync(Account);
            if (!validationResult.IsValid)
            {
                var errorDetails = new HttpValidationProblemDetails(validationResult.ToDictionary());
                return BadRequest(errorDetails);
            }
            var result = await _accountService.Login(Account);
            if (!result.Succeeded)
            {
                var errorDetails = new HttpValidationProblemDetails(
                    result.Errors.ToDictionary(
                        e => e.Code,
                        e => new string[] { e.Description }));
                return BadRequest(errorDetails);
            }
            var token = await _accountService.GeneratToken(Account.Email);
            return Ok(token);
            
        }
    }
}
