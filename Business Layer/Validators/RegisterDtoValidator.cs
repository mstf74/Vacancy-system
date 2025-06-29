using Business_Layer.Dtos;
using Data_Access_Layer.Enums;
using Data_Access_Layer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        UserManager<ApplicationUser> _userManager;
        public RegisterDtoValidator(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
            RuleFor(u => u.Email)
                .EmailAddress();
            RuleFor(u => u.UserName)
                .NotEmpty()
                .MustAsync(UniqueUserName).WithMessage("User name is already taken");
            RuleFor(u => u.phonenumber)
                .NotEmpty()
                .Must(ValidPhoneForm).WithErrorCode("invalidphoneForm").WithMessage("Invalid phone number")
                .Length(11)
                .MustAsync(UniquePhoneNumber).WithMessage("Phone number is already taken");
            RuleFor(u => u.Rule)
                .Must(ValidRule).WithMessage("The Rule must be either Employer or Applicant");
        }
        public async Task<bool> UniqueUserName(string userName, CancellationToken ct)
        {
            var result = await _userManager.FindByNameAsync(userName);
            if (result is null)
                return true;
            return false;

        }
        private async Task<bool> UniquePhoneNumber(string phoneNumber, CancellationToken ct)
        {
            return! await _userManager.Users.AnyAsync( u => u.PhoneNumber == phoneNumber);
        }
        private bool ValidRule(Rules rule)
        {
            if (rule == Rules.Applicant || rule == Rules.Employer)
                return true;
            return false;
        }
        private bool ValidPhoneForm(string phonenumber)
        {
            bool result = int.TryParse(phonenumber, out int _);
            return result;
        }
    }
}
