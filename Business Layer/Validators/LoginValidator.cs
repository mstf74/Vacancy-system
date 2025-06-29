using Business_Layer.Dtos;
using Data_Access_Layer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginValidator(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
            RuleFor(u => u.Email)
                .EmailAddress()
                .MustAsync(ExistedUser).WithMessage("Email is not registerd");
        }
        private async Task<bool> ExistedUser(string email, CancellationToken ct)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result is not null) 
                return true;
            return false;
        }
    }
}
